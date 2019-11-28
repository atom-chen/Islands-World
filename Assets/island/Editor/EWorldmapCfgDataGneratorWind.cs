using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System.IO;

/// <summary>
/// EW orldmap cfg data gnerator wind.大地图配置数据生成器
/// </summary>
public class EWorldmapCfgDataGneratorWind : EditorWindow
{
    string cfgPath = "Assets/" + CLPathCfg.self.basePath + "/Editor/worldMap/worldMapGenratorCfg.asset";
    WorldmapEditorCfg editorCfg;
    bool isInited = false;
    Vector2 scrollPos = Vector2.zero;
    Color tmpColor = Color.white;
    int tmpOceanGid = 0;

    public class MapTile
    {
        int index = 0;      //网格index
        int id = 0;         //配置id

    }

    //生成器的配置
    public class WorldmapEditorCfg
    {
        public int size = 1000;
        public int pageSize = 10;
        public string mapAreaTexturePath = "";
        Texture2D _mapAreaTexture;
        public Texture2D oceanGidTexture
        {
            get
            {
                if (_mapAreaTexture == null)
                {
                    _mapAreaTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(mapAreaTexturePath);
                }
                return _mapAreaTexture;
            }
            set
            {
                _mapAreaTexture = value;
                mapAreaTexturePath = AssetDatabase.GetAssetPath(value);
            }
        }
        string _colors4MapAreaJson = "";
        string colors4MapAreaJson
        {
            set
            {
                if (_colors4MapAreaJson != value)
                {
                    _colors4MapArea = null;
                }
                _colors4MapAreaJson = value;
            }
            get
            {
                Hashtable m = new Hashtable();
                if (colors4MapArea != null)
                {
                    foreach (var cell in colors4MapArea)
                    {
                        m[JSON.JsonEncode(Utl.colorToMap(cell.Key))] = cell.Value;
                        _colors4MapAreaJson = JSON.JsonEncode(m);
                    }
                }
                return _colors4MapAreaJson;
            }
        }
        Dictionary<Color, int> _colors4MapArea;
        public Dictionary<Color, int> colors4MapArea
        {
            get
            {
                if (_colors4MapArea == null)
                {
                    _colors4MapArea = new Dictionary<Color, int>();
                    Hashtable map = JSON.DecodeMap(_colors4MapAreaJson);
                    if (map != null)
                    {
                        foreach (DictionaryEntry cell in map)
                        {
                            Color cl = Utl.mapToColor(JSON.DecodeMap(cell.Key.ToString()));
                            _colors4MapArea.Add(cl, NumEx.stringToInt(cell.Value.ToString()));
                        }
                    }
                }
                return _colors4MapArea;
            }
        }
        public static WorldmapEditorCfg parse(Hashtable map)
        {
            WorldmapEditorCfg cfg = new WorldmapEditorCfg();
            cfg.size = MapEx.getInt(map, "size");
            cfg.pageSize = MapEx.getInt(map, "pageSize");
            cfg.mapAreaTexturePath = MapEx.getString(map, "mapAreaTexturePath");
            cfg.colors4MapAreaJson = MapEx.getString(map, "colors4MapAreaJson");
            return cfg;
        }

        public Hashtable toMap()
        {
            Hashtable map = new Hashtable();
            map["size"] = size;
            map["pageSize"] = pageSize;
            map["mapAreaTexturePath"] = mapAreaTexturePath;
            map["colors4MapAreaJson"] = colors4MapAreaJson;
            return map;
        }
    }

    public void init()
    {
        if (isInited) return;
        isInited = true;
        Hashtable mapCfg = new Hashtable();
        TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(cfgPath);
        if (ta != null)
        {
            Debug.Log(ta.text);
            mapCfg = JSON.DecodeMap(ta.text);
        }
        editorCfg = WorldmapEditorCfg.parse(mapCfg);
    }
    void saveEditorCfg()
    {
        string json = JSON.JsonEncode(editorCfg.toMap());
        TextAsset ta = new TextAsset(json);
        Debug.Log(ta.text);
        AssetDatabase.CreateAsset(ta, cfgPath);
        Debug.Log("Save success!!");
    }
    //=================================================================
    //=================================================================
    //=================================================================
    void OnGUI()
    {
        init();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            showAttrs();

            if (GUILayout.Button("Save Config"))
            {
                saveEditorCfg();
            }
            if (GUILayout.Button("Generate World Map Data!"))
            {
                if (EditorUtility.DisplayDialog("Alert", "Really!?", "Okey", "cancel"))
                {
                    generateData();
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void showAttrs()
    {
        if (editorCfg == null)
        {
            isInited = false;
            init();
        }
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("地图大小", ECLEditorUtl.width200);
            editorCfg.size = EditorGUILayout.IntField(editorCfg.size);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("一屏的大小", ECLEditorUtl.width200);
            editorCfg.pageSize = EditorGUILayout.IntField(editorCfg.pageSize);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("地图分区的色块图", ECLEditorUtl.width200);

            editorCfg.oceanGidTexture =
                        EditorGUILayout.ObjectField(
                        editorCfg.oceanGidTexture,
                        typeof(Texture2D), false) as Texture2D;
            if (GUILayout.Button("Reset Mode"))
            {
                resetTextrueMode();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("颜色值与地图分区值的对应", ECLEditorUtl.width200);
        ECLEditorUtl.BeginContents();
        {
            GUILayout.BeginVertical();
            {
                foreach (var cell in editorCfg.colors4MapArea)
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.ColorField(cell.Key);
                        EditorGUILayout.IntField(cell.Value);
                        if (GUILayout.Button("-"))
                        {
                            editorCfg.colors4MapArea.Remove(cell.Key);
                            break;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUI.color = Color.yellow;
                GUILayout.BeginHorizontal();
                {
                    tmpColor = EditorGUILayout.ColorField(tmpColor);
                    tmpOceanGid = EditorGUILayout.IntField(tmpOceanGid);
                    if (GUILayout.Button("+"))
                    {
                        editorCfg.colors4MapArea[tmpColor] = tmpOceanGid;
                        tmpOceanGid = 0;
                    }
                }
                GUILayout.EndHorizontal();
                GUI.color = Color.white;
            }
            GUILayout.EndVertical();
        }
        ECLEditorUtl.EndContents();
    }

    void resetTextrueMode()
    {
        TextureImporter ti = AssetImporter.GetAtPath(editorCfg.mapAreaTexturePath) as TextureImporter;
        if (ti == null)
        {
            Debug.LogError("get TextureImporter is null!" + editorCfg.mapAreaTexturePath);
            return;
        }
        TextureImporterPlatformSettings tips = ti.GetPlatformTextureSettings("Standalone");
        Debug.LogError(tips.format);
        tips.format = TextureImporterFormat.RGBA32;
        ti.SetPlatformTextureSettings(tips);
        ti.textureType = TextureImporterType.GUI;
        ti.isReadable = true;
        EditorUtility.SetDirty(ti);
        AssetDatabase.WriteImportSettingsIfDirty(editorCfg.mapAreaTexturePath);
        AssetDatabase.ImportAsset(editorCfg.mapAreaTexturePath);
    }

    void generateData()
    {
        if (editorCfg == null)
        {
            Debug.Log("Editor config is null");
            return;
        }

        // 设置grid
        GridBase grid = new GridBase();
        grid.init(Vector3.zero, editorCfg.size, editorCfg.size, 1);

        // 先把图的格式设置成RGBA32
        Texture2D areaTex = editorCfg.oceanGidTexture;
        if (areaTex == null)
        {
            Debug.Log("oceanGid Texture is null");
            return;
        }

        // 先取得大地图分区的数据
        Color32[] colors = areaTex.GetPixels32();
        Debug.Log("areaTex.width===" + areaTex.width);
        Debug.Log("colors.Length===" + colors.Length);
        // 区域块的index对应的val
        Hashtable areaMap = new Hashtable();
        // 取得大地图与分区地图的比例，以例可以对换
        int mapareaScale = editorCfg.size / areaTex.width;

        //地图分区块的网格
        GridBase gridArea = new GridBase();
        gridArea.init(Vector3.zero, areaTex.width, areaTex.width, mapareaScale);

        Color c;
        for (int i = 0; i < colors.Length - 1; i++)
        {
            c = colors[i];
            int areaVal = 0;
            if (editorCfg.colors4MapArea.TryGetValue(c, out areaVal))
            {
                areaMap[i] = areaVal;
            }
            else
            {
                Debug.LogError("get area val by color is nil!" + c + "==" + c * 255);
            }
        }

        //地图数据
        Hashtable mapInfor = new Hashtable();
        // 生成据点
        int tmpSize = editorCfg.pageSize * 2;
        for (int i = tmpSize / 2; i < editorCfg.size; i += tmpSize)
        {
            for (int j = tmpSize / 2; j < editorCfg.size; j += tmpSize)
            {
                int index = grid.GetCellIndex(i, j);
                List<int> cells = grid.getCells(index, 5);
                int k = NumEx.NextInt(0, cells.Count);
                while (true)
                {
                    if (k >= cells.Count)
                    {
                        k = 0;
                    }

                    if (mapInfor[(int)(cells[k])] == null)
                    {
                        mapInfor[(int)(cells[k])] = 1;
                        break;
                    }
                    k++;
                }

            }
        }
        // 生成装饰
        for (int i = editorCfg.pageSize / 2; i < editorCfg.size; i = i + editorCfg.pageSize)
        {
            for (int j = editorCfg.pageSize / 2; j < editorCfg.size; j = j + editorCfg.pageSize)
            {
                int index = grid.GetCellIndex(i, j);
                switch (NumEx.NextInt(0, 4))
                {
                    case 0:
                        addIsland1(grid, index, mapInfor);
                        break;
                    case 1:
                        addIsland2(grid, index, mapInfor);
                        break;
                    case 2:
                        addIsland3(grid, index, mapInfor);
                        break;
                    default:
                        addIsland4(grid, index, mapInfor);
                        break;
                }

            }
        }
        //导出数据（会根据分区导出数据，这里会导出100份数据，方便加载）
        /*
         * 每个分区id对应一个文件，每个文件里存一个table，key是一屏的index，value是table2
         * table2里的key是网格index，value是地块配置id
         */       
        string path = Application.dataPath + "/" + CLPathCfg.self.basePath + "/" + CLPathCfg.upgradeRes + "/priority/cfg/worldmap/maparea.cfg";
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        MemoryStream ms = new MemoryStream();
        B2OutputStream.writeObject(ms, areaMap);
        File.WriteAllBytes(path, ms.ToArray());
        Debug.Log(path);
        for (int i = 0; i < areaTex.width; i++)
        {
            for (int j = 0; j < areaTex.width; j++)
            {
                int index = i * areaTex.width + j;
                Hashtable areaPageMap = new Hashtable();
                List<int> pagesIndes = areaIndex2MapPageIndexs(grid, gridArea, index, mapareaScale);
                for(int p = 0; p < pagesIndes.Count; p++)
                {
                    int center = pagesIndes[p];
                    List<int> cells = grid.getCells(center, editorCfg.pageSize);
                    Hashtable map = new Hashtable();
                    for (int k = 0; k < cells.Count; k++)
                    {
                        if (mapInfor[cells[k]] != null)
                        {
                            map[cells[k]] = mapInfor[cells[k]];
                        }
                    }
                    areaPageMap[center] = map;
                }
                path = Application.dataPath + "/" + CLPathCfg.self.basePath + "/" + CLPathCfg.upgradeRes + "/priority/cfg/worldmap/" + index + ".cfg";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                ms = new MemoryStream();
                B2OutputStream.writeObject(ms, areaPageMap);
                File.WriteAllBytes(path, ms.ToArray());
            }
        }

        Debug.Log("total===" + mapInfor.Count);
    }

    void occupyCells(GridBase grid, List<int> cells, int id, int size, Hashtable map)
    {
        int count = 0;
        int i = NumEx.NextInt(0, cells.Count);
        int index = 0;
        while (true)
        {
            if (i >= cells.Count)
            {
                i = 0;
            }
            index = cells[i];
            if (size > 1)
            {
                bool canAdd = true;
                List<int> _cells = grid.getCells(index, size);
                for (int j = 0; j < _cells.Count; j++)
                {
                    if (_cells[j] < 0 || map[_cells[j]] != null)
                    {
                        canAdd = false;
                        break;
                    }
                }
                if (canAdd)
                {
                    for (int j = 0; j < _cells.Count; j++)
                    {
                        map[_cells[j]] = 99; //占用
                    }
                    map[index] = id;
                    break;
                }
            }
            else
            {
                if (map[index] == null)
                {
                    map[index] = id;
                    break;
                }
            }
            i++;
            count++;
            if (count >= cells.Count)
            {
                break;
            }
        }
    }

    //1-5个占一个格子及1-2个占4个格子的装饰
    void addIsland1(GridBase grid, int index, Hashtable map)
    {
        List<int> cells = grid.getCells(index, editorCfg.pageSize);
        int count = NumEx.NextInt(0, 2);
        for (int i = 0; i <= count; i++)
        {
            int id = NumEx.NextBool() ? 4 : 5;
            occupyCells(grid, cells, id, 2, map);
        }
        count = NumEx.NextInt(0, 5);
        for (int i = 0; i <= count; i++)
        {
            int id = NumEx.NextBool() ? 2 : 3;
            occupyCells(grid, cells, id, 1, map);
        }
    }
    //1-5个占一个格子的及1个占9个格子的装饰
    void addIsland2(GridBase grid, int index, Hashtable map)
    {
        List<int> cells = grid.getCells(index, editorCfg.pageSize);
        occupyCells(grid, cells, 6, 4, map);

        int count = NumEx.NextInt(0, 5);
        for (int i = 0; i <= count; i++)
        {
            int id = NumEx.NextBool() ? 2 : 3;
            occupyCells(grid, cells, id, 1, map);
        }
    }
    //6-12个占一个格子装饰
    void addIsland3(GridBase grid, int index, Hashtable map)
    {
        List<int> cells = grid.getCells(index, editorCfg.pageSize);
        int count = NumEx.NextInt(5, 12);
        for (int i = 0; i <= count; i++)
        {
            int id = NumEx.NextBool() ? 2 : 3;
            occupyCells(grid, cells, id, 1, map);
        }
    }
    //2-4个占4个格子的装饰
    void addIsland4(GridBase grid, int index, Hashtable map)
    {
        List<int> cells = grid.getCells(index, editorCfg.pageSize);
        int count = NumEx.NextInt(2, 5);
        for (int i = 0; i <= count; i++)
        {
            int id = NumEx.NextBool() ? 2 : 3;
            occupyCells(grid, cells, id, 1, map);
        }
    }

    /// <summary>
    /// Mapcell2s the index of the area.取得大图的index映射到分区网格的index
    /// </summary>
    /// <returns>The area index.</returns>
    /// <param name="grid">Grid.</param>
    /// <param name="gridArea">Grid area.</param>
    /// <param name="index">Index.</param>
    /// <param name="scale">Scale.</param>
    int mapIndex2AreaIndex(GridBase grid, GridBase gridArea, int index, int scale)
    {
        int areaIndex = -1;
        int col = grid.GetColumn(index);
        int row = grid.GetRow(index);
        col = col / scale;
        row = row / scale;

        areaIndex = gridArea.GetCellIndex(col, row);
        return areaIndex;
    }

    /// 分区网格的index转成大地图每屏的index
    List<int> areaIndex2MapPageIndexs(GridBase grid, GridBase gridArea, int areaIndex, int scale)
    {
        List<int> ret = new List<int>();
        int col = gridArea.GetColumn(areaIndex);
        int row = gridArea.GetRow(areaIndex);
        col = col * scale;
        row = row * scale;
        for (int i = col + editorCfg.pageSize / 2; i < col + scale - 1; i = i + editorCfg.pageSize)
        {
            for (int j = row + editorCfg.pageSize / 2; j < row + scale - 1; j = j + editorCfg.pageSize)
            {
                ret.Add(grid.GetCellIndex(i, j));
            }
        }
        return ret;
    }
}
