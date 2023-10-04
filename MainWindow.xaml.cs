using mercenary_data_editor.Components;
using mercenary_data_editor.Model.Spawn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model;
using mercenary_data_editor.Model.Item;
using mercenary_data_editor.Model.Partner;
using mercenary_data_editor.Model.Player;
using mercenary_data_editor.Model.Store;
using mercenary_data_editor.Model.Weapon;

namespace mercenary_data_editor
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public BaseModel model { get; }

    private List<TabItem> enemyDataTabs = new();
    private List<TabItem> spawnDataTabs = new();
    private List<TabItem> acDataTabs = new();
    private List<TabItem> partnerDataTabs = new();
    private List<TabItem> weaponDataTabs = new();
    private List<TabItem> itemDataTabs = new();

    public MainWindow()
    {
      InitializeComponent();
      c_path.Text = AppDomain.CurrentDomain.BaseDirectory;
      c_waveTime_list.ItemsSource = seconds;
      c_player_status.ItemsSource = playerStatus;
      model = new BaseModel();
      DataContext = model;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => LoadData();

    private void Save_OnClick(object sender, RoutedEventArgs e) => SaveData();

    private void Load_OnClick(object sender, RoutedEventArgs e) => LoadData();

    private void OpenJson(object sender, RoutedEventArgs e)
    {
      var select = c_mainTabControl.SelectedIndex switch
      {
        0 => "AttributeChemistryData",
        1 or 2 or 3 => "SpawnData",
        4 => "PartnerData",
        5 => "WeaponData",
        6 => "ItemData",
        7 => "PlayerData",
        8 or 9 or 10 => "StoreData",
        _ => throw new ArgumentOutOfRangeException()
      };
      Process.Start(new ProcessStartInfo($@"{c_path.Text}\Data\{select}.json")
      {
        UseShellExecute = true
      });
    }

    #region Core

    private T LoadJson<T>(string fileName)
    {
      using var sr = new StreamReader($@"{c_path.Text}\Data\{fileName}.json");
      return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
    }

    private void SaveJson(string fileName, object obj)
    {
      using var sw = new StreamWriter($@"{c_path.Text}\Data\{fileName}.json");
      sw.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
    }

    private void LoadData()
    {
      AttributeChemistryData attributeChemistryData;
      Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>> attributeChemistrySimplyData;
      SpawnData spawnData;
      PartnerData partnerData;
      WeaponData weaponData;
      ItemData itemData;
      PlayerStatusData playerStatusData;
      StoreData storeData;

      try
      {
        // Clear Data
        ClearACData();
        ClearEnemyData();
        ClearSpawnData();
        ClearWaveTimeData();
        ClearPartnerData();
        ClearWeaponData();
        ClearItemData();
        ClearPlayerStatusData();
        c_refreshPrice.Clear();
        c_waveProbability.Clear();
        c_storeProbability.Clear();

        // Attribute Chemistry Data
        attributeChemistryData = LoadJson<AttributeChemistryData>("AttributeChemistryData");

        attributeChemistrySimplyData = attributeChemistryData.ToSimply();

        foreach (var (attribute, dict) in attributeChemistrySimplyData)
        {
          CreateACData(attribute, dict);
        }

        // Enemy Data / Spawn Data
        spawnData = LoadJson<SpawnData>("SpawnData");

        foreach (var ed in spawnData.enemyData)
        {
          CreateEnemyData
          (
            ed.name,
            (
              ed.defaultStatus.hp,
              ed.defaultStatus.damage,
              ed.defaultStatus.moveSpeed,
              ed.defaultStatus.dropCoin
            ),
            (
              ed.increaseStatus.hp,
              ed.increaseStatus.damage,
              ed.increaseStatus.moveSpeed,
              ed.increaseStatus.dropCoin
            ),
            ed.detailStatus.Select(x => new EnemyDetailStatus()
            {
              status = x.status.ToString(), value = x.value
            }).ToArray()
          );
        }

        foreach (var sd in spawnData.spawns)
        {
          CreateSpawnData
          (
            sd.spawns
              .Select(x => new Enemy(x.name, x.count, x.simultaneousSpawnCount, x.delay, x.range))
              .ToArray()
          );
        }

        foreach (var wtd in spawnData.waveTime)
        {
          CreateWaveTimeData(wtd);
        }

        // Partner
        partnerData = LoadJson<PartnerData>("PartnerData");

        foreach (var partner in partnerData.data)
        foreach (var status in partner.status)
        foreach (var applyStatus in status.applyStatus)
        foreach (var value in applyStatus.status)
          CreatePartnerData(partner.name, partner.status.Select(x => new PartnerStatusData()
          {
            attribute = status.attribute.ToString(),
            tier = applyStatus.tier,
            status = value.type.ToString(),
            value = value.value
          }).ToArray());

        // Weapons
        weaponData = LoadJson<WeaponData>("WeaponData");

        foreach (var weapon in weaponData.weapons)
        {
          CreateWeaponData
          (
            weapon.name.ParseEnum<Weapons>(),
            weapon.attribute,
            weapon.tiers.Select(x => x.status.Select(y => (y.type, y.value)).ToArray()).ToArray()
          );
        }

        // Items
        itemData = LoadJson<ItemData>("ItemData");

        foreach (var item in itemData.items)
        {
          CreateItemData
          (
            item.name,
            item.tier,
            item.applies.Select(x => (x.type, x.value)).ToArray()
          );
        }

        // Player Status
        playerStatusData = LoadJson<PlayerStatusData>("PlayerData");

        foreach (var status in playerStatusData.status)
          CreatePlayerStatusData(status.type, status.value);

        // Store Data
        storeData = LoadJson<StoreData>("StoreData");

        foreach (var price in storeData.refreshPrices)
          c_refreshPrice.Add(price);
        foreach (var probability in storeData.tierProbabilities)
          c_storeProbability.Add(probability);
        foreach (var probability in storeData.additionalProbabilitiesOfWaves)
          c_waveProbability.Add(probability);
        
        MessageBox.Show("Loading Complete!", "Load", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Load Failed.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void SaveData()
    {
      try
      {
        // Weapon Attribute Chemistry Data
        var resACSimplyData = new Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>>();

        foreach (var tabItem in acDataTabs)
        {
          var acd = (AttributeChemistry)tabItem.Content;
          var attr = (Attribute)Enum.Parse(typeof(Attribute), acd.c_type.Text);
          var value = new Dictionary<int, Dictionary<ApplyStatus, float>>();
          foreach (var a in acd.model.attributes)
          {
            var type = (ApplyStatus)Enum.Parse(typeof(ApplyStatus), a.type);

            if (value.ContainsKey(a.count))
              value[a.count].Add(type, a.amount);
            else
              value.Add(a.count, new Dictionary<ApplyStatus, float>() { { type, a.amount } });
          }

          if (!resACSimplyData.TryAdd(attr, value))
            throw new Exception("attribute type is must be single element.");
        }

        var resACData = new AttributeChemistryData().Parse(resACSimplyData);

        // Enemy Data
        var resSpawnData = new SpawnData();
        var enemyData = new List<SpawnData.Enemy>();

        foreach (var tabItem in enemyDataTabs)
        {
          var ed = (EnemyData)tabItem.Content;
          enemyData.Add(new SpawnData.Enemy()
          {
            name = ed.model.name,
            defaultStatus = new SpawnData.Enemy.Status()
            {
              hp = ed.model.defHp,
              damage = ed.model.defDamage,
              moveSpeed = ed.model.defMoveSpeed,
              dropCoin = ed.model.defDropCoin,
            },
            increaseStatus = new SpawnData.Enemy.Status()
            {
              hp = ed.model.incHp,
              damage = ed.model.incDamage,
              moveSpeed = ed.model.incMoveSpeed,
              dropCoin = ed.model.incDropCoin,
            },
            detailStatus = ed.model.details.Select
            (
              x => new SpawnData.Enemy.Detail()
              {
                status = x.status.ParseEnum<SpawnData.Enemy.Detail.DetailStatus>(),
                value = x.value
              }).ToArray()
          });
        }

        resSpawnData.enemyData = enemyData.ToArray();

        // Spawn Data
        var spawnData = new List<SpawnData.Spawns>();

        foreach (var tabItem in spawnDataTabs)
        {
          var sd = (SpawnWave)tabItem.Content;
          spawnData.Add(new SpawnData.Spawns()
          {
            spawns = sd.model.enemies.Select(x => new SpawnData.Spawns.Spawn()
              {
                count = x.count,
                name = x.name,
                simultaneousSpawnCount = x.simultaneousSpawnCount,
                delay = x.delay,
                range = x.range
              })
              .ToArray()
          });
        }

        resSpawnData.spawns = spawnData.ToArray();

        // Wave Time
        resSpawnData.waveTime = seconds.Select(x => x.second).ToArray();

        SaveJson("AttributeChemistryData", resACData);

        SaveJson("SpawnData", resSpawnData);

        // Partner
        var resPartnerSimplyData =
          new Dictionary<string, Dictionary<Attribute, Dictionary<int, (PartnerData.Status status, string value)[]>>>();

        foreach (var model in partnerDataTabs.Select(x => ((Partner)x.Content).model))
        {
          var pFirst = new Dictionary<Attribute, Dictionary<int, List<(PartnerData.Status status, string value)>>>();

          foreach (var partnerStatusData in model.list)
          {
            var attr = partnerStatusData.attribute.ParseEnum<Attribute>();
            var tier = partnerStatusData.tier;
            var apply = (partnerStatusData.status.ParseEnum<PartnerData.Status>(), partnerStatusData.value);

            if (pFirst.ContainsKey(attr))
              if (pFirst[attr].ContainsKey(tier))
                pFirst[attr][tier].Add(apply);
              else
                pFirst[attr].Add(tier, new List<(PartnerData.Status status, string value)>() { apply });
            else
              pFirst.Add(attr, new Dictionary<int, List<(PartnerData.Status status, string value)>>()
              {
                { tier, new List<(PartnerData.Status status, string value)>() { apply } }
              });
          }

          foreach (var (key, value) in pFirst)
          {
            var v = value.ToDictionary(x => x.Key, x => x.Value.ToArray());
            if (resPartnerSimplyData.ContainsKey(model.name))
              resPartnerSimplyData[model.name].Add(key, v);
            else
              resPartnerSimplyData.Add(model.name,
                new Dictionary<Attribute, Dictionary<int, (PartnerData.Status status, string value)[]>>()
                {
                  { key, v }
                });
          }
        }

        SaveJson("PartnerData", new PartnerData().Parse(resPartnerSimplyData));

        // Weapon
        var resWeaponData =
          new Dictionary<string, (Attribute attribute, Dictionary<int, Dictionary<WeaponStatusItem, float>> tiers)>();

        foreach (var weaponTab in weaponDataTabs.Select(x => (Weapon)x.Content))
        {
          var name = weaponTab.model.weaponType;
          Attribute attr = 0;

          foreach (var a in weaponTab.model.attributeList.Select(x => x.attribute.ParseEnum<Attribute>()))
            attr |= a;

          var dict = weaponTab.c_tab.Items
            .Cast<TabItem>()
            .ToDictionary
            (
              x => Convert.ToInt32(x.Header),
              x => ((Weapon2)x.Content).model.list.ToDictionary
              (
                y => y.status.ParseEnum<WeaponStatusItem>(),
                y => y.value
              )
            );

          resWeaponData.Add(name, (attr, dict));
        }

        SaveJson("WeaponData", new WeaponData().Parse(resWeaponData));

        // Item
        var resItemData = new Dictionary<string, (int tier, Dictionary<ItemStatusItem, float> apply)>();

        foreach (var itemTab in itemDataTabs.Select(x => (Item)x.Content))
        {
          var name = itemTab.model.itemType;
          var tier = itemTab.model.tier;

          var dict = itemTab.model.list.ToDictionary(x => x.status.ParseEnum<ItemStatusItem>(), x => x.value);
          resItemData.Add(name, (tier, dict));
        }

        SaveJson("ItemData", new ItemData().Parse(resItemData));

        // Player Status
        var resPlayerStatusData = new Dictionary<PlayerStatusItem, float>();

        foreach (var status in playerStatus)
          resPlayerStatusData.Add(status.status.ParseEnum<PlayerStatusItem>(), status.value);

        SaveJson("PlayerData", new PlayerStatusData().Parse(resPlayerStatusData));

        // Store
        var resRefreshPrices = c_refreshPrice.model.prices.Select(x => x.price).ToArray();
        var resTierProbabilities = c_storeProbability.model.probabilities.Select(x => x.probability).ToArray();
        var resWaveProbabilities = c_waveProbability.model.probabilities.Select(x => x.probability).ToArray();
        
        SaveJson("StoreData", new StoreData
        {
          refreshPrices = resRefreshPrices,
          tierProbabilities = resTierProbabilities,
          additionalProbabilitiesOfWaves = resWaveProbabilities
        });

        MessageBox.Show("Save Complete!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Save Failed.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    #endregion

    #region Attribute Chemistry

    private void CreateACData(Attribute defaultAttribute = default,
      Dictionary<int, Dictionary<ApplyStatus, float>> data = null)
    {
      var item = new TabItem()
      {
        Header = acDataTabs.Count
      };
      var acd = new AttributeChemistry()
      {
        Margin = new Thickness(0, 0, 0, 0),
        tabItem = item,
      };

      if (data is not null)
      {
        acd.c_type.SelectedItem = defaultAttribute.ToString();
        foreach (var (count, dict1) in data)
        {
          foreach (var (status, amount) in dict1)
          {
            acd.model.attributes.Add(new AttributeChemistryItem(count, status.ToString(), amount));
          }
        }
      }
      else
      {
        acd.c_type.SelectedItem = Attribute.None.ToString();
      }

      item.Content = acd;

      c_attribute_tab.Items.Add(item);
      acDataTabs.Add(item);
      item.Focus();
    }

    private void RemoveACData(int index)
    {
      c_attribute_tab.Items.RemoveAt(index);
      acDataTabs.RemoveAt(index);
    }

    public void ClearACData()
    {
      c_attribute_tab.Items.Clear();
      acDataTabs.Clear();
    }

    private void AttributeChemistry_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreateACData();
    }

    private void AttributeChemistry_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      RemoveACData(c_attribute_tab.SelectedIndex);
    }

    #endregion

    #region Enemy

    private void CreateEnemyData
    (
      string name = "New Enemy",
      (float hp, float damage, float moveSpeed, float dropCoin) defStatus = default,
      (float hp, float damage, float moveSpeed, float dropCoin) incStatus = default,
      EnemyDetailStatus[] detailStatus = default
    )
    {
      var item = new TabItem()
      {
        Header = name
      };

      var ed = new EnemyData
      (
        name,
        defStatus == default ? (0, 0, 0, 0) : defStatus,
        incStatus == default ? (0, 0, 0, 0) : incStatus,
        detailStatus == default ? Array.Empty<EnemyDetailStatus>() : detailStatus
      )
      {
        Margin = new Thickness(0, 0, 0, 0),
        tabItem = item
      };

      item.Content = ed;

      c_enemyData_tab.Items.Add(item);
      enemyDataTabs.Add(item);
      item.Focus();
    }

    private void ClearEnemyData()
    {
      c_enemyData_tab.Items.Clear();
      enemyDataTabs.Clear();
    }

    private void RemoveEnemyData(int index)
    {
      c_enemyData_tab.Items.RemoveAt(index);
      enemyDataTabs.RemoveAt(index);
    }

    private void AddEnemyData_OnClick(object sender, RoutedEventArgs e)
    {
      CreateEnemyData();
    }

    private void RemoveEnemyData_OnClick(object sender, RoutedEventArgs e)
    {
      try
      {
        RemoveEnemyData(c_enemyData_tab.SelectedIndex);
      }
      catch
      {
      }
    }

    #endregion

    #region Spawn

    private void SpawnSetting_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreateSpawnData();
    }

    public void CreateSpawnData(Enemy[] enemies = null)
    {
      if (enemies == null) enemies = Array.Empty<Enemy>();
      var item = new TabItem()
      {
        Header = spawnDataTabs.Count
      };
      var sd = new SpawnWave(enemyDataTabs)
      {
        Margin = new Thickness(0, 0, 0, 0),
      };

      foreach (var enemy in enemies)
      {
        sd.model.enemies.Add(enemy);
      }

      //ed.OnNameChanged += (i, value) => enemyDataTabs[i].Header = value;
      item.Content = sd;

      c_spawn_tab.Items.Add(item);
      spawnDataTabs.Add(item);
      item.Focus();
    }

    private void RemoveSpawnData(int index)
    {
      c_spawn_tab.Items.RemoveAt(index);
      spawnDataTabs.RemoveAt(index);

      for (var i = index; i < spawnDataTabs.Count; i++)
      {
        var sw = (SpawnWave)spawnDataTabs[i].Content;
        spawnDataTabs[i].Header = Convert.ToInt32(spawnDataTabs[i].Header) - 1;
      }
    }

    public void ClearSpawnData()
    {
      c_spawn_tab.Items.Clear();
      spawnDataTabs.Clear();
    }

    private void SpawnSetting_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      try
      {
        RemoveSpawnData(c_spawn_tab.SelectedIndex);
      }
      catch
      {
      }
    }

    #endregion

    #region Wave Time

    private ObservableCollection<WaveTime> seconds = new();

    private void CreateWaveTimeData(float second)
    {
      seconds.Add(new WaveTime(seconds.Count, second));
    }

    private void ClearWaveTimeData()
    {
      seconds.Clear();
    }

    private void WaveTime_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreateWaveTimeData(seconds.Count > 0 ? seconds[^1].second : 0f);
    }

    private void WaveTime_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      try
      {
        if (c_waveTime_list.SelectedIndex == -1) return;
        for (var i = c_waveTime_list.SelectedIndex + 1; i < seconds.Count; i++)
        {
          seconds[i] = new WaveTime(seconds[i].wave - 1, seconds[i].second);
        }

        seconds.RemoveAt(c_waveTime_list.SelectedIndex);
      }
      catch
      {
      }
    }

    #endregion

    #region Partner

    private void Partner_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreatePartnerData($"New Partner {partnerDataTabs.Count}");
    }

    private void Partner_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      if (c_partner_tab.SelectedIndex == -1) return;
      RemovePartnerData(c_partner_tab.SelectedIndex);
    }

    public void CreatePartnerData(string name = "", PartnerStatusData[] data = null)
    {
      if (data == null) data = Array.Empty<PartnerStatusData>();

      TabItem item;
      Partner pd;

      if (partnerDataTabs.Any(x => ((Partner)x.Content).model.name == name))
      {
        item = partnerDataTabs.Single(x => ((Partner)x.Content).model.name == name);
        pd = (Partner)item.Content;
      }
      else
      {
        item = new TabItem()
        {
          Header = partnerDataTabs.Count
        };
        pd = new Partner(item, name)
        {
          Margin = new Thickness(0, 0, 0, 0),
        };
        item.Content = pd;
        c_partner_tab.Items.Add(item);
        partnerDataTabs.Add(item);
      }

      foreach (var value in data)
      {
        pd.model.list.Add(value);
      }

      item.Focus();
    }

    private void RemovePartnerData(int index)
    {
      c_partner_tab.Items.RemoveAt(index);
      partnerDataTabs.RemoveAt(index);
    }

    private void ClearPartnerData()
    {
      c_partner_tab.Items.Clear();
      partnerDataTabs.Clear();
    }

    #endregion

    #region Weapon

    private void Weapon_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreateWeaponData();
    }

    private void Weapon_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      if (c_weapon_tab.SelectedIndex == -1) return;
      RemoveWeaponData(c_weapon_tab.SelectedIndex);
    }

    public void CreateWeaponData
    (
      Weapons weapon = Weapons.ballista,
      Attribute attribute = Attribute.None,
      (WeaponStatusItem status, float value)[][] data = null
    )
    {
      var item = new TabItem()
      {
        Header = weaponDataTabs.Count
      };
      var wd = new Weapon(item, weapon, attribute)
      {
        Margin = new Thickness(0, 0, 0, 0),
      };

      if (data != null)
        foreach (var x in data)
          wd.AddItem(x);

      item.Content = wd;

      c_weapon_tab.Items.Add(item);
      weaponDataTabs.Add(item);
      item.Focus();
    }

    private void RemoveWeaponData(int index)
    {
      c_weapon_tab.Items.RemoveAt(index);
      weaponDataTabs.RemoveAt(index);
    }

    private void ClearWeaponData()
    {
      c_weapon_tab.Items.Clear();
      weaponDataTabs.Clear();
    }

    #endregion

    #region Item

    private void Item_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreateItemData();
    }

    private void Item_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      if (c_items_tab.SelectedIndex == -1) return;
      RemoveItemData(c_items_tab.SelectedIndex);
    }

    public void CreateItemData
    (
      string itemName = null,
      int tier = 0,
      (ItemStatusItem status, float value)[] data = null
    )
    {
      if (string.IsNullOrEmpty(itemName)) itemName = Items.barbell.ToString();
      var item = new TabItem()
      {
        Header = weaponDataTabs.Count
      };
      var wd = new Item(item, itemName)
      {
        Margin = new Thickness(0, 0, 0, 0),
      };

      if (data != null)
      {
        wd.model.tier = tier;
        foreach (var x in data)
          wd.AddItem(x.status.ToString(), x.value);
      }

      item.Content = wd;

      c_items_tab.Items.Add(item);
      itemDataTabs.Add(item);
      item.Focus();
    }

    private void RemoveItemData(int index)
    {
      c_items_tab.Items.RemoveAt(index);
      itemDataTabs.RemoveAt(index);
    }

    private void ClearItemData()
    {
      c_items_tab.Items.Clear();
      itemDataTabs.Clear();
    }

    #endregion

    #region Player Status

    private ObservableCollection<PlayerStatusModelData> playerStatus { get; set; } = new();

    private void PlayerStatus_Add_OnClick(object sender, RoutedEventArgs e)
    {
      CreatePlayerStatusData();
    }

    private void PlayerStatus_Remove_OnClick(object sender, RoutedEventArgs e)
    {
      if (c_player_status.SelectedIndex == -1) return;
      playerStatus.RemoveAt(c_player_status.SelectedIndex);
    }

    private void CreatePlayerStatusData(PlayerStatusItem type = PlayerStatusItem.MaxHp, float value = 0f)
    {
      playerStatus.Add(new PlayerStatusModelData() { status = type.ToString(), value = value });
    }

    private void ClearPlayerStatusData()
    {
      playerStatus.Clear();
    }

    #endregion
  }
}