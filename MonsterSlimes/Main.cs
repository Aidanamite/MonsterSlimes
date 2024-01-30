using HarmonyLib;
using SRML;
using SRML.Console;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using SRML.SR;
using SRML.SR.Translation;
using SRML.Utils.Enum;
using Console = SRML.Console.Console;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MonsterSlimes
{
    public class Main : ModEntryPoint
    {
        internal static Assembly modAssembly = Assembly.GetExecutingAssembly();
        internal static string modName = $"{modAssembly.GetName().Name}";
        internal static string modDir = $"{System.Environment.CurrentDirectory}\\SRML\\Mods\\{modName}";
        internal static Dictionary<Identifiable.Id, Sprite> sprites = new Dictionary<Identifiable.Id, Sprite>();
        internal static Transform prefabParent;

        public Main()
        {
            var p = new GameObject("PrefabParent");
            p.SetActive(false);
            Object.DontDestroyOnLoad(p);
            prefabParent = p.transform;
        }

        public override void PreLoad()
        {
            HarmonyInstance.PatchAll();
            BasicSlimeRegistry(
                Ids.STOMACH_SLIME, Ids2.STOMACH_SLIME,
                null,//LoadImage("slime.png", 512, 512).CreateSprite(),
                "Stomach Slime",
                "Intro",
                "Food",
                "Favourite",
                "Slimeology",
                "Risks",
                "Plortinomics",
                null
                );
            BasicSlimeRegistry(
                Ids.SEA_CUCUMBER_SLIME, Ids2.SEA_CUCUMBER_SLIME,
                null,//LoadImage("slime.png", 512, 512).CreateSprite(),
                "Sea Cucumber Slime",
                "Intro",
                "Food",
                "Favourite",
                "Slimeology",
                "Risks",
                "Plortinomics",
                null
                );
            BasicSlimeRegistry(
                Ids.SOFT_TISSUE_SLIME, Ids2.SOFT_TISSUE_SLIME,
                null,//LoadImage("slime.png", 512, 512).CreateSprite(),
                "Soft Tissue Slime",
                "Intro",
                "Food",
                "Favourite",
                "Slimeology",
                "Risks",
                "Plortinomics",
                null
                );
            BasicSlimeRegistry(
                Ids.WORM_SLIME, Ids2.WORM_SLIME,
                null,//LoadImage("slime.png", 512, 512).CreateSprite(),
                "Worm Slime",
                "Intro",
                "Food",
                "Favourite",
                "Slimeology",
                "Risks",
                "Plortinomics",
                null
                );
            BasicSlimeRegistry(
                Ids.CREAM_SLIME, Ids2.CREAM_SLIME,
                null,//LoadImage("slime.png", 512, 512).CreateSprite(),
                "Cream Slime",
                "Intro",
                "Food",
                "Favourite",
                "Slimeology",
                "Risks",
                "Plortinomics",
                null
                );
            BasicSlimeRegistry(
                Ids.WEATHER_SLIME, Ids2.WEATHER_SLIME,
                null,//LoadImage("slime.png", 512, 512).CreateSprite(),
                "Weather Slime",
                "Intro",
                "Food",
                "Favourite",
                "Slimeology",
                "Risks",
                "Plortinomics",
                null
                );
        }

        public override void Load()
        {
            var pinkDef = GameContext.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PINK_SLIME);
            var tabbyDef = GameContext.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.TABBY_SLIME);

            #region Gulpin
            var def = Object.Instantiate(pinkDef);
            def.AppearancesDefault = new SlimeAppearance[] { Object.Instantiate(def.AppearancesDefault[0]) };
            def.CanLargofy = false;
            def.Diet = new SlimeDiet()
            {
                AdditionalFoods = pinkDef.Diet.AdditionalFoods,
                Favorites = new Identifiable.Id[0],
                FavoriteProductionCount = 2,
                MajorFoodGroups = pinkDef.Diet.MajorFoodGroups,
                Produces = new Identifiable.Id[] { Identifiable.Id.WATER_LIQUID }// { Ids.STOMACH_PLORT }
            };
            def.FavoriteToys = new Identifiable.Id[0];
            def.IdentifiableId = Ids.STOMACH_SLIME;
            def.IsLargo = false;
            def.Name = "Stomach Slime";
            def.name = "StomachSlime";
            def.Diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            var a = def.AppearancesDefault[0];
            var blinkEye = pinkDef.AppearancesDefault[0].Face.ExpressionFaces.First((x) => x.SlimeExpression == SlimeFace.SlimeExpression.Blink).Eyes;
            a.ChangeFaceColors(Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, a.Face.ExpressionFaces.All((x) => true, (x) => new SlimeExpressionFace() { Eyes = blinkEye, SlimeExpression = x.SlimeExpression }).ToArray());
            
            a.Structures = new SlimeAppearanceStructure[] {
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0])
                };
            a.ColorPalette.Ammo = new Color(0.7f, 0.85f, 0.7f);
            a.ColorPalette.Top = new Color(0.7f, 0.85f, 0.7f);
            a.ColorPalette.Middle = new Color(0.65f, 0.85f, 0.65f);
            a.ColorPalette.Bottom = new Color(0.5f, 0.7f, 0.5f);

            var pinkBodyApp = a.Structures[0].Element.Prefabs[0];
            a.Structures[0].Element = CreateElement("slimeStomachBase", pinkBodyApp.CreatePrefab());
            var bodyObj = a.Structures[0].Element.Prefabs[0].gameObject;
            bodyObj.name = "slime_drag";
            var skin = bodyObj.GetComponent<SkinnedMeshRenderer>();
            var mesh = Object.Instantiate(skin.sharedMesh);
            mesh.name = "slime_drag";
            var v = mesh.vertices;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i].y < 0.5f && v[i].z < 0)
                    v[i] += Vector3.back * ((0.5f - Mathf.Sin(Mathf.Abs(v[i].x) * Mathf.PI)) * (1 - v[i].y * 2) + 0.2f) * 0.5f;
                v[i] = v[i].Multiply(1.2f,0.8f,1.2f);
            }
            mesh.vertices = v;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            skin.sharedMesh = mesh;


            var body = a.Structures[0].DefaultMaterials[0].Clone();
            body.name = "slimeStomachBase";
            a.Structures[0].DefaultMaterials[0] = body;
            body.SetColor("_TopColor", new Color(0.7f, 0.85f, 0.7f));
            body.SetColor("_MiddleColor", new Color(0.65f, 0.85f, 0.65f));
            body.SetColor("_BottomColor", new Color(0.5f, 0.7f, 0.5f));

            var body2 = blinkEye.Clone();
            a.Structures[0].DefaultMaterials = a.Structures[0].DefaultMaterials.AddToArray(body2);
            body2.name = "slimeStomachBack";
            body2.SetColor("_EyeRed", Color.black);
            body2.SetColor("_EyeGreen", Color.black);
            body2.SetColor("_EyeBlue", Color.black);
            body2.SetTexture("_FaceAtlas", LoadImage("slimeStomach_back.png", 1024, 512));
            body2.SetTextureOffset("_texcoord", body2.GetTextureOffset("_texcoord") + new Vector2(0.26f,-0.1f));// * 1.02f);
            body2.SetTextureScale("_texcoord", new Vector2(0.5f,1));
            a.Icon = sprites[Ids.STOMACH_SLIME];


            a.Structures[1].DefaultMaterials = new Material[] { a.Structures[0].DefaultMaterials[0].Clone() };
            var feather = a.Structures[1].DefaultMaterials[0];
            feather.name = "slimeStomachBack";
            feather.SetColor("_TopColor", new Color(1, 1, 0));
            feather.SetColor("_MiddleColor", new Color(1, 1, 0));
            feather.SetColor("_BottomColor", new Color(1, 1, 0));

            a.Structures[1].Element = CreateElement("slimeStomachFeather",new GameObject("").CreatePrefab().AddComponent<SlimeAppearanceObject>());
            var featherObj = a.Structures[1].Element.Prefabs[0];
            featherObj.ParentBone = SlimeAppearance.SlimeBone.JiggleTop;
            featherObj.RootBone = SlimeAppearance.SlimeBone.Root;
            featherObj.IgnoreLODIndex = true;
            mesh = new Mesh();
            var step = 8;
            var ver = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(0, 0.5f, 0) };
            var tri = new List<int>
            {
                0,2,3,
                0,3,4,
                0,4,5,
                0,5,2
            };
            var uv = new List<Vector2> { new Vector2(0, 1) };
            for (int i = 1; i < step; i++)
            {
                var y = 1f / step * i;
                uv.Add(new Vector2(0, y));
                uv.Add(new Vector2(0, y));
                uv.Add(new Vector2(0, y));
                uv.Add(new Vector2(0, y));
                var x = Mathf.Sqrt(1 - Mathf.Pow(y * 2 - 1,2));
                y *= ver[1].y;
                ver.Add(new Vector3(x * 0.125f, y, 0));
                ver.Add(new Vector3(0, y, -x * 0.05f));
                ver.Add(new Vector3(-x * 0.125f, y, 0));
                ver.Add(new Vector3(0, y, x * 0.05f));
                if (i > 1)
                    tri.AddRange(new int[]
                    {
                        ver.Count - 8, ver.Count - 4, ver.Count - 3,
                        ver.Count - 8, ver.Count - 3, ver.Count - 7,
                        ver.Count - 7, ver.Count - 3, ver.Count - 2,
                        ver.Count - 7, ver.Count - 2, ver.Count - 6,
                        ver.Count - 6, ver.Count - 2, ver.Count - 1,
                        ver.Count - 6, ver.Count - 1, ver.Count - 5,
                        ver.Count - 5, ver.Count - 1, ver.Count - 4,
                        ver.Count - 5, ver.Count - 4, ver.Count - 8
                    }
                    );
            }
            for (int i = 0; i < ver.Count; i++)
                ver[i] -= Vector3.up * 0.25f;
            tri.AddRange(new int[]
            {
                1,ver.Count - 1,ver.Count - 2,
                1,ver.Count - 2,ver.Count - 3,
                1,ver.Count - 3,ver.Count - 4,
                1,ver.Count - 4,ver.Count - 1
            });
            mesh.vertices = ver.ToArray();
            mesh.triangles = tri.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            featherObj.gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
            featherObj.gameObject.AddComponent<MeshRenderer>();


            var slimePrefab = GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).CreatePrefab();
            slimePrefab.name = "slimeStomach";
            var id = slimePrefab.GetComponent<Identifiable>();
            id.id = Ids.STOMACH_SLIME;
            var app = slimePrefab.GetComponent<SlimeAppearanceApplicator>();
            app.SlimeDefinition = def;
            app.Appearance = a;
            GenerateBoneData(app, a.Structures[0].Element.Prefabs[0], 0.5f);
            var eat = slimePrefab.GetComponent<SlimeEat>();
            eat.slimeDefinition = def;
            LookupRegistry.RegisterIdentifiablePrefab(id);
            SlimeRegistry.RegisterSlimeDefinition(def);
            #endregion Gulpin
            //-------------------------------------------------------------------------------------------------------------

            #region Pyukumuku
            def = Object.Instantiate(pinkDef);
            def.AppearancesDefault = new SlimeAppearance[] { Object.Instantiate(def.AppearancesDefault[0]) };
            def.CanLargofy = false;
            def.Diet = new SlimeDiet()
            {
                AdditionalFoods = pinkDef.Diet.AdditionalFoods,
                Favorites = new Identifiable.Id[0],
                FavoriteProductionCount = 2,
                MajorFoodGroups = pinkDef.Diet.MajorFoodGroups,
                Produces = new Identifiable.Id[] { Identifiable.Id.WATER_LIQUID }// { Ids.SEA_CUCUMBER_PLORT }
            };
            def.FavoriteToys = new Identifiable.Id[0];
            def.IdentifiableId = Ids.SEA_CUCUMBER_SLIME;
            def.IsLargo = false;
            def.Name = "Sea Cucumber Slime";
            def.name = "SeaCucumberSlime";
            def.Diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            a = def.AppearancesDefault[0];
            var customMouth = pinkDef.AppearancesDefault[0].Face.ExpressionFaces.First((x) => x.SlimeExpression == SlimeFace.SlimeExpression.Happy).Mouth.Clone();
            customMouth.SetTexture("_FaceAtlas", LoadImage("slimeSeaCucumber_mouth.png", 64, 64, TextureWrapMode.Clamp));
            a.ChangeFaceColors(
                new Color(0.85f, 0.4f, 0.65f), new Color(0.85f, 0.4f, 0.65f), new Color(0.85f, 0.4f, 0.65f),
                Color.white, Color.white, Color.white,
                a.Face.ExpressionFaces.All((x) =>
                x.SlimeExpression != SlimeFace.SlimeExpression.Awe &&
                x.SlimeExpression != SlimeFace.SlimeExpression.AttackTelegraph &&
                x.SlimeExpression != SlimeFace.SlimeExpression.Alarm &&
                x.SlimeExpression != SlimeFace.SlimeExpression.ChompOpen,
                (x) => new SlimeExpressionFace() { Mouth = customMouth, SlimeExpression = x.SlimeExpression }).ToArray());

            a.Structures = new SlimeAppearanceStructure[] {
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0])
                };
            a.ColorPalette.Ammo = new Color(0.275f, 0.275f, 0.275f);
            a.ColorPalette.Top = new Color(0.275f, 0.275f, 0.275f);
            a.ColorPalette.Middle = new Color(0.275f, 0.275f, 0.275f);
            a.ColorPalette.Bottom = new Color(0.1875f, 0.1875f, 0.1875f);

            a.Structures[0].Element = CreateElement("slimeSeaCucumberBase", pinkBodyApp.CreatePrefab());
            bodyObj = a.Structures[0].Element.Prefabs[0].gameObject;
            bodyObj.name = "slime_squash";
            skin = bodyObj.GetComponent<SkinnedMeshRenderer>();
            mesh = Object.Instantiate(skin.sharedMesh);
            mesh.name = "slime_squash";
            v = mesh.vertices;
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = v[i].Multiply(1.2f, 0.8f, 1.2f);
            }
            mesh.vertices = v;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            skin.sharedMesh = mesh;


            body = a.Structures[0].DefaultMaterials[0].Clone();
            body.name = "slimeSeaCucumberBase";
            a.Structures[0].DefaultMaterials[0] = body;
            body.SetColor("_TopColor", new Color(0.275f, 0.275f, 0.275f));
            body.SetColor("_MiddleColor", new Color(0.275f, 0.275f, 0.275f));
            body.SetColor("_BottomColor", new Color(0.1875f, 0.1875f, 0.1875f));
            a.Icon = sprites[Ids.SEA_CUCUMBER_SLIME];


            a.Structures[1].DefaultMaterials = new Material[] { a.Structures[0].DefaultMaterials[0].Clone() };
            var top = a.Structures[1].DefaultMaterials[0];
            top.name = "slimeSeaCucumberTop";
            top.SetColor("_TopColor", new Color(0.85f, 0.4f, 0.65f));
            top.SetColor("_MiddleColor", new Color(0.85f, 0.4f, 0.65f));
            top.SetColor("_BottomColor", new Color(0.55f, 0.3f, 0.35f));

            a.Structures[1].Element = CreateElement("slimeSeaCucumberTop", pinkBodyApp.CreatePrefab());
            var topObj = a.Structures[1].Element.Prefabs[0];
            topObj.name = "slimeSeaCucumberTop";
            step = 32;
            var step2 = 16;
            ver = new List<Vector3> { new Vector3(0, 0.5f, 0) };
            tri = new List<int>();
            uv = new List<Vector2> { new Vector2(0, 1) };
            for (int j = 0; j < step2; j++)
                tri.AddRange(new int[] { 0, j + 1, (j + 1) % step2 + 1 });
            for (int i = step - 1; i >= 0; i--)
            {
                var y = 1f / step * i;
                for (int j = 0; j < step2; j++)
                    uv.Add(new Vector2(0, y * 0.5f + 0.5f));
                var x = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
                y *= ver[0].y;
                for (int j = 0; j < step2; j++)
                    ver.Add(new Vector3(x * 0.125f, y, 0).Rotate(0, 360f / step2 * j, 0));
                if (i < step - 1)
                    for (int j = 0; j < step2; j++)
                        tri.AddRange(new int[]
                        {
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + j, ver.Count - step2 + (j + 1) % step2,
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + (j + 1) % step2, ver.Count - (step2 * 2) + (j + 1) % step2
                        }
                        );
            }
            for (int i = 0; i < ver.Count; i++)
                ver[i] += Vector3.up * 0.7f;
            topObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = CreateMesh(ver.ToArray(), tri.ToArray(), uv.ToArray(), new Func<Vector3, Vector3>[] {
                (x) => x.Rotate(0,0,20),
                (x) => x.Rotate(0,0,-20),
                (x) => x.Rotate(25,0,15) * 0.8f + new Vector3(0, 0.2f,0.1f),
                (x) => x.Rotate(25,0,-15) * 0.8f + new Vector3(0, 0.2f,0.1f),
                (x) => x.Rotate(-25,0,15) * 0.8f + new Vector3(0, 0.2f,-0.1f),
                (x) => x.Rotate(-25,0,-15) * 0.8f + new Vector3(0, 0.2f,-0.1f)
            }, "slimeSeaCucumberTop");


            a.Structures[2].DefaultMaterials = new Material[] { a.Structures[0].DefaultMaterials[0].Clone() };
            var dome = a.Structures[2].DefaultMaterials[0];
            dome.name = "slimeSeaCucumberTail";
            dome.SetColor("_TopColor", Color.white);
            dome.SetColor("_MiddleColor", Color.white);
            dome.SetColor("_BottomColor", Color.white);

            a.Structures[2].Element = CreateElement("slimeSeaCucumberTail", pinkBodyApp.CreatePrefab());
            var tailObj = a.Structures[2].Element.Prefabs[0];
            tailObj.name = "slimeSeaCucumberTail";
            step = 8;
            step2 = 16;
            ver = new List<Vector3> { new Vector3(0, 0.1f, 0) };
            tri = new List<int>();

            for (int j = 0; j < step2; j++)
                tri.AddRange(new int[] { 0, j + 1, (j + 1) % step2 + 1 });
            for (int i = step - 1; i >= 0; i--)
            {
                var y = 1f / step * i;
                var x = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
                y *= ver[0].y;
                for (int j = 0; j < step2; j++)
                    ver.Add(new Vector3(x * 0.1f, y, 0).Rotate(0, 360f / step2 * j, 0));
                if (i < step - 1)
                    for (int j = 0; j < step2; j++)
                        tri.AddRange(new int[]
                        {
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + j, ver.Count - step2 + (j + 1) % step2,
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + (j + 1) % step2, ver.Count - (step2 * 2) + (j + 1) % step2
                        }
                        );
            }
            for (int i = 0; i < ver.Count; i++)
                ver[i] += Vector3.up * 0.1f;
            var f = new List<Func<Vector3, Vector3>>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 6; j++)
                {
                    if ((i == 0 || i == 3) && j > 0)
                        continue;
                    var k = i;
                    var l = j;
                    f.Add((x) => x.Rotate(0, 60 * l, 60 * k) + new Vector3(0, 0.4f, -0.6f));
                }
            tailObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = CreateMesh(ver.ToArray(), tri.ToArray(), new Vector2[ver.Count], f.ToArray(), "slimeSeaCucumberTail");

            slimePrefab = GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).CreatePrefab();
            slimePrefab.name = "slimeSeaCucumber";
            id = slimePrefab.GetComponent<Identifiable>();
            id.id = Ids.SEA_CUCUMBER_SLIME;
            app = slimePrefab.GetComponent<SlimeAppearanceApplicator>();
            app.SlimeDefinition = def;
            app.Appearance = a;
            GenerateBoneData(app, a.Structures[0].Element.Prefabs[0], 0.3f, 1, null, topObj, tailObj);
            eat = slimePrefab.GetComponent<SlimeEat>();
            eat.slimeDefinition = def;
            LookupRegistry.RegisterIdentifiablePrefab(id);
            SlimeRegistry.RegisterSlimeDefinition(def);
            #endregion Pyukumuku
            //-------------------------------------------------------------------------------------------------------------

            #region Goomy
            def = Object.Instantiate(pinkDef);
            def.AppearancesDefault = new SlimeAppearance[] { Object.Instantiate(def.AppearancesDefault[0]) };
            def.CanLargofy = false;
            def.Diet = new SlimeDiet()
            {
                AdditionalFoods = pinkDef.Diet.AdditionalFoods,
                Favorites = new Identifiable.Id[0],
                FavoriteProductionCount = 2,
                MajorFoodGroups = pinkDef.Diet.MajorFoodGroups,
                Produces = new Identifiable.Id[] { Identifiable.Id.WATER_LIQUID }// { Ids.STOMACH_PLORT }
            };
            def.FavoriteToys = new Identifiable.Id[0];
            def.IdentifiableId = Ids.SOFT_TISSUE_SLIME;
            def.IsLargo = false;
            def.Name = "Soft Tissue Slime";
            def.name = "SoftTissueSlime";
            def.Diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            a = def.AppearancesDefault[0];
            a.ChangeFaceColors(
                Color.black, Color.black, Color.black,
                new Color(0.45f, 0.4f, 0.45f), new Color(0.45f, 0.4f, 0.45f), new Color(0.2f, 0.15f, 0.2f),
                a.Face.ExpressionFaces.All((x) => x.Eyes, (x) => {
                    var ma = x.Eyes.Clone();
                    ma.SetTextureScale("_texcoord", x.Eyes.GetTextureScale("_texcoord").Multiply(2f, 1));
                    ma.SetTextureOffset("_texcoord", x.Eyes.GetTextureOffset("_texcoord") + new Vector2(-0.5f, -0.1f));
                    return new SlimeExpressionFace() { Eyes = ma, SlimeExpression = x.SlimeExpression };
                }).ToArray());
            var mouthAwe = a.Face.GetExpressionFace(SlimeFace.SlimeExpression.Awe).Mouth.Clone();
            mouthAwe.name = "mouthAweSoftTissue";
            mouthAwe.SetTexture("_FaceAtlas", LoadImage("slimeSoftTissue_mouthAwe.png", 512, 512));
            var mouthElated = a.Face.GetExpressionFace(SlimeFace.SlimeExpression.Elated).Mouth.Clone();
            mouthElated.name = "mouthElatedSoftTissue";
            mouthElated.SetTexture("_FaceAtlas", LoadImage("slimeSoftTissue_mouthElated.png", 512, 512));
            var mouthFeral = a.Face.GetExpressionFace(SlimeFace.SlimeExpression.Feral).Mouth.Clone();
            mouthFeral.name = "mouthFeralSoftTissue";
            mouthFeral.SetTexture("_FaceAtlas", LoadImage("slimeSoftTissue_mouthFeral.png", 512, 512));
            var mouthHungry = a.Face.GetExpressionFace(SlimeFace.SlimeExpression.Hungry).Mouth.Clone();
            mouthHungry.name = "mouthHungrySoftTissue";
            mouthHungry.SetTexture("_FaceAtlas", LoadImage("slimeSoftTissue_mouthHungry.png", 512, 512));
            var mouthNone = a.Face.GetExpressionFace(SlimeFace.SlimeExpression.Hungry).Mouth.Clone();
            mouthNone.name = "mouthNoneSoftTissue";
            mouthNone.SetTexture("_FaceAtlas", LoadImage("slimeSoftTissue_mouthNone.png", 512, 512));
            for (int i = 0; i < a.Face.ExpressionFaces.Length; i++)
            {
                var e = a.Face.ExpressionFaces[i];
                if (e.SlimeExpression  == SlimeFace.SlimeExpression.Angry || e.SlimeExpression == SlimeFace.SlimeExpression.Feral)
                    e.Mouth = mouthFeral;
                else if (e.SlimeExpression == SlimeFace.SlimeExpression.ChompOpen || e.SlimeExpression == SlimeFace.SlimeExpression.AttackTelegraph || e.SlimeExpression == SlimeFace.SlimeExpression.Elated)
                    e.Mouth = mouthElated;
                else if (e.SlimeExpression == SlimeFace.SlimeExpression.Awe || e.SlimeExpression == SlimeFace.SlimeExpression.Scared || e.SlimeExpression == SlimeFace.SlimeExpression.Alarm)
                    e.Mouth = mouthAwe;
                else if (e.SlimeExpression == SlimeFace.SlimeExpression.Hungry)
                    e.Mouth = mouthHungry;
                else
                    e.Mouth = mouthNone;
                a.Face.ExpressionFaces[i] = e;
            }
            a.Face.OnEnable();

            a.Structures = new SlimeAppearanceStructure[] {
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0])
                };
            a.ColorPalette.Ammo = new Color(0.7f, 0.85f, 0.7f);
            a.ColorPalette.Top = new Color(0.7f, 0.85f, 0.7f);
            a.ColorPalette.Middle = new Color(0.65f, 0.85f, 0.65f);
            a.ColorPalette.Bottom = new Color(0.5f, 0.7f, 0.5f);

            a.Structures[0].Element = CreateElement("slimeSoftTissueBase", pinkBodyApp.CreatePrefab());
            bodyObj = a.Structures[0].Element.Prefabs[0].gameObject;
            bodyObj.name = "slime_drag2";
            skin = bodyObj.GetComponent<SkinnedMeshRenderer>();
            mesh = Object.Instantiate(skin.sharedMesh);
            mesh.name = "slime_drag2";
            v = mesh.vertices;
            var u = mesh.uv;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i].y < 0.5f)
                {
                    v[i].y = 0;
                    if (v[i].z < 0)
                        v[i].z = v[i].z * 1.5f;
                    u[i].y = 0;
                }
            }
            mesh.vertices = v;
            mesh.uv = u;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            skin.sharedMesh = mesh;


            body = tabbyDef.AppearancesDefault[0].Structures[0].DefaultMaterials[0].Clone();
            body.name = "slimeSoftTissueBase";
            a.Structures[0].DefaultMaterials[0] = body;
            body.SetColor("_TopColor", new Color(0.85f, 0.8f, 0.9f));
            body.SetColor("_MiddleColor", new Color(0.85f, 0.8f, 0.9f));
            body.SetColor("_BottomColor", new Color(0.7f, 0.6f, 0.75f));
            body.SetTexture("_StripeTexture", LoadImage("slimeSoftTissue_stripe.png", 512, 512));
            body.SetTextureScale("_texcoord", new Vector2(0.5f, 1));

            body2 = blinkEye.Clone();
            a.Structures[0].DefaultMaterials = a.Structures[0].DefaultMaterials.AddToArray(body2);
            body2.name = "slimeSoftTissueCheeks";
            body2.SetColor("_EyeRed", new Color(0.45f, 0.7f, 0.45f).Multiply(0.6f));
            body2.SetColor("_EyeGreen", new Color(0.45f, 0.7f, 0.45f).Multiply(0.6f));
            body2.SetColor("_EyeBlue", new Color(0.35f, 0.55f, 0.35f).Multiply(0.6f));
            body2.SetTexture("_FaceAtlas", LoadImage("slimeSoftTissue_cheeks.png", 512, 512));
            a.Icon = sprites[Ids.SOFT_TISSUE_SLIME];

            Log("4");
            a.Structures[1].DefaultMaterials = new Material[] { a.Structures[0].DefaultMaterials[0].Clone() };
            var antenna = a.Structures[1].DefaultMaterials[0];
            antenna.name = "slimeSoftTissueAntenna";
            antenna.SetColor("_TopColor", new Color(0.85f, 0.8f, 0.9f));
            antenna.SetColor("_MiddleColor", new Color(0.85f, 0.8f, 0.9f));
            antenna.SetColor("_BottomColor", new Color(0.85f, 0.8f, 0.9f));
            a.Structures[1].Element = CreateElement("slimeSoftTissueAntenna", pinkBodyApp.CreatePrefab());
            var antennaObj = a.Structures[1].Element.Prefabs[0];
            antennaObj.name = "slimeSoftTissueAntenna";
            step = 32;
            step2 = 32;
            ver = new List<Vector3> { new Vector3(0, 0.5f, 0) };
            tri = new List<int>();
            uv = new List<Vector2> { new Vector2(0, 1) };
            for (int j = 0; j < step2; j++)
                tri.AddRange(new int[] { 0, j + 1, (j + 1) % step2 + 1 });
            for (int i = step - 1; i >= 0; i--)
            {
                var y = 1f / step * i;
                for (int j = 0; j < step2; j++)
                    uv.Add(new Vector2(0,y));
                var x = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
                y *= ver[0].y;
                for (int j = 0; j < step2; j++)
                    ver.Add(new Vector3(x * 0.05f, y, 0).Rotate(0, 360f / step2 * j, 0));
                if (i < step - 1)
                    for (int j = 0; j < step2; j++)
                        tri.AddRange(new int[]
                        {
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + j, ver.Count - step2 + (j + 1) % step2,
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + (j + 1) % step2, ver.Count - (step2 * 2) + (j + 1) % step2
                        }
                        );
            }
            for (int i = 0; i < ver.Count; i++)
            {
                var m = Math.Max(Mathf.Sqrt(Mathf.Min(ver[i].y * 2, 1)), 0.5f) * 2;
                ver[i] = ver[i].Multiply(m, 1, m).Rotate(-ver[i].y * 30, 0, 0, 0, 0, -0.5f);
            }
            antennaObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = CreateMesh(ver.ToArray(), tri.ToArray(), uv.ToArray(), new Func<Vector3, Vector3>[] {
                (x) => x.Multiply(0.8f, 0.4f, 0.8f).Rotate(35, 0, 0).Offset(0, 0.45f, 0).Rotate(30,0,20).Offset(0, 0.5f, 0),
                (x) => x.Multiply(0.8f, 0.4f, 0.8f).Rotate(35, 0, 0).Offset(0, 0.45f, 0).Rotate(30,0,-20).Offset(0, 0.5f, 0),
                (x) => x.Rotate(-10, 0, 0).Offset(0, 0.45f, 0).Rotate(30,0,20).Offset(0, 0.5f, 0),
                (x) => x.Rotate(-10, 0, 0).Offset(0, 0.45f, 0).Rotate(30,0,-20).Offset(0, 0.5f, 0)
            }, "slimeSoftTissueAntenna");


            slimePrefab = GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).CreatePrefab();
            slimePrefab.name = "slimeSoftTissue";
            id = slimePrefab.GetComponent<Identifiable>();
            id.id = Ids.SOFT_TISSUE_SLIME;
            app = slimePrefab.GetComponent<SlimeAppearanceApplicator>();
            app.SlimeDefinition = def;
            app.Appearance = a;
            GenerateBoneData(app, a.Structures[0].Element.Prefabs[0], 0.5f, 1, null, antennaObj);
            eat = slimePrefab.GetComponent<SlimeEat>();
            eat.slimeDefinition = def;
            LookupRegistry.RegisterIdentifiablePrefab(id);
            SlimeRegistry.RegisterSlimeDefinition(def);
            #endregion Goomy
            //-------------------------------------------------------------------------------------------------------------

            #region Snom
            def = Object.Instantiate(pinkDef);
            def.AppearancesDefault = new SlimeAppearance[] { Object.Instantiate(def.AppearancesDefault[0]) };
            def.CanLargofy = false;
            def.Diet = new SlimeDiet()
            {
                AdditionalFoods = pinkDef.Diet.AdditionalFoods,
                Favorites = new Identifiable.Id[0],
                FavoriteProductionCount = 2,
                MajorFoodGroups = pinkDef.Diet.MajorFoodGroups,
                Produces = new Identifiable.Id[] { Identifiable.Id.WATER_LIQUID }// { Ids.SEA_CUCUMBER_PLORT }
            };
            def.FavoriteToys = new Identifiable.Id[0];
            def.IdentifiableId = Ids.WORM_SLIME;
            def.IsLargo = false;
            def.Name = "Worm Slime";
            def.name = "WormSlime";
            def.Diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            a = def.AppearancesDefault[0];
            customMouth = pinkDef.AppearancesDefault[0].Face.ExpressionFaces.First((x) => x.SlimeExpression == SlimeFace.SlimeExpression.Happy).Mouth.Clone();
            customMouth.SetTexture("_FaceAtlas", LoadImage("slimeWorm_mouth.png", 64, 64, TextureWrapMode.Clamp));
            a.ChangeFaceColors(
                Color.black, Color.black, Color.black,
                Color.white, Color.white, Color.white,
                a.Face.ExpressionFaces.All((x) => true,
                (x) => {
                    var e = x.Eyes.Clone();
                    e.SetTextureOffset("_texcoord", e.GetTextureOffset("_texcoord") + new Vector2(0,0.15f));
                    return new SlimeExpressionFace() { Mouth = customMouth, SlimeExpression = x.SlimeExpression, Eyes = e };
                }).ToArray());

            a.Structures = new SlimeAppearanceStructure[] {
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0])
                };
            a.ColorPalette.Ammo = Color.white;
            a.ColorPalette.Top = Color.white;
            a.ColorPalette.Middle = Color.white;
            a.ColorPalette.Bottom = new Color(0.9f, 0.9f, 1);

            a.Structures[0].Element = CreateElement("slimeWormBase", pinkBodyApp.CreatePrefab());
            bodyObj = a.Structures[0].Element.Prefabs[0].gameObject;
            bodyObj.name = "slime_squash2";
            skin = bodyObj.GetComponent<SkinnedMeshRenderer>();
            mesh = Object.Instantiate(skin.sharedMesh);
            mesh.name = "slime_squash2";
            v = mesh.vertices;
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = v[i].Multiply(0.8f, 0.8f, 1.2f);
            }
            mesh.vertices = v;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            skin.sharedMesh = mesh;


            body = a.Structures[0].DefaultMaterials[0].Clone();
            body.name = "slimeWormBase";
            a.Structures[0].DefaultMaterials[0] = body;
            body.SetColor("_TopColor", new Color(0.9f, 0.9f, 0.9f));
            body.SetColor("_MiddleColor", new Color(0.9f, 0.9f, 0.9f));
            body.SetColor("_BottomColor", new Color(0.6f, 0.6f, 0.6f));
            a.Icon = sprites[Ids.WORM_SLIME];


            a.Structures[1].DefaultMaterials = new Material[] { Resources.FindObjectsOfTypeAll<Material>().First((x) => x.name == "glassScreens").Clone() };
            a.Structures[1].SupportsFaces = false;
            var shell = a.Structures[1].DefaultMaterials[0];
            shell.name = "slimeWormShell";
            shell.SetTexture("_MainTex", LoadImage("slimeWorm_glass.png", 512, 512));
            shell.SetTexture("_BumpMap", LoadImage("slimeWorm_normal.png", 512, 512));
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.clear);
            tex.Apply();
            shell.SetTexture("_EmissionMap", tex);
            //shell.SetFloatArray("_BlurAmount", new float[] { 0 });
            //shell.SetFloat("_WaveOffset", 0);


            a.Structures[1].Element = CreateElement("slimeWormShell", a.Structures[0].Element.Prefabs[0].CreatePrefab());
            var shellObj = a.Structures[1].Element.Prefabs[0];
            shellObj.name = "slimeWormShell";
            mesh = Object.Instantiate(shellObj.GetComponent<SkinnedMeshRenderer>().sharedMesh);
            mesh.name = "slimeWormShell";

            var d = new Vector3[]
            {
                new Vector3(1,0,0),
                new Vector3(-1,0,0),
                new Vector3(1,1,0),
                new Vector3(-1,1,0),
                new Vector3(0,1,0),
                new Vector3(1,0,-1),
                new Vector3(-1,0,-1),
                new Vector3(1,1,-1),
                new Vector3(-1,1,-1),
                new Vector3(0,1,-1),
                new Vector3(0,0,-1)
            };
            for (int i = 0; i < d.Length; i++)
                d[i] = d[i].normalized;
            var d2 = d.All((x) => true, (x) => float.PositiveInfinity).ToArray();
            var ind = new int[d.Length];
            v = mesh.vertices;
            for (int i = 0; i < v.Length; i++)
                for (int j = 0; j < d.Length; j++)
                {
                    var o = (v[i].Offset(0, -0.3f, 0).normalized - d[j]).sqrMagnitude;
                    if (o < d2[j])
                    {
                        d2[j] = o;
                        ind[j] = i;
                    }
                }
            ver = v.ToList();
            for (int i = 0; i < v.Length; i++)
            {
                for (int j = 0; j < d.Length; j++)
                if (ind[j] == i || (ver[ind[j]] - ver[i]).magnitude < 0.01f)
                    v[i] += v[i].Offset(0,-0.3f,0).normalized * 0.2f;
                v[i] = v[i].Multiply(1.2f, 1.2f, 1).Offset(0, -0.05f, -0.1f);
            }
            mesh.vertices = v;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            shellObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;


            a.Structures[2].DefaultMaterials = new Material[] { a.Structures[0].DefaultMaterials[0] };
            a.Structures[2].SupportsFaces = false;

            a.Structures[2].Element = CreateElement("slimeWormFace", pinkBodyApp.CreatePrefab());
            var faceObj = a.Structures[2].Element.Prefabs[0];
            faceObj.name = "slimeWormFace";
            step = 8;
            step2 = 16;
            ver = new List<Vector3> { new Vector3(0, 0.3f, 0) };
            tri = new List<int>();
            uv = new List<Vector2> { new Vector2(0, 0.3f) };
            for (int j = 0; j < step2; j++)
                tri.AddRange(new int[] { 0, j + 1, (j + 1) % step2 + 1 });
            for (int i = step - 1; i >= 0; i--)
            {
                var y = 1f / step * i;
                var x = Mathf.Sqrt(1 - Mathf.Pow(y, 2));

                y *= ver[0].y;
                for (int j = 0; j < step2; j++)
                    ver.Add(new Vector3(x * 0.15f, y, 0).Rotate(0, 360f / step2 * j, 0));
                for (int j = 0; j < step2; j++)
                    uv.Add(new Vector2(0, 0.3f - ver[ver.Count - step2 + j].z * 2 ));
                if (i < step - 1)
                    for (int j = 0; j < step2; j++)
                        tri.AddRange(new int[]
                        {
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + j, ver.Count - step2 + (j + 1) % step2,
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + (j + 1) % step2, ver.Count - (step2 * 2) + (j + 1) % step2
                        }
                        );
            }
            faceObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = CreateMesh(ver.ToArray(), tri.ToArray(), uv.ToArray(), new Func<Vector3, Vector3>[] {
                (x) => x.Rotate(90,0,0).Offset(-0.1f,0.3f,0.4f),
                (x) => x.Rotate(90,0,0).Offset(0.1f,0.3f,0.4f)
            }, "slimeWormFace");

            slimePrefab = GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).CreatePrefab();
            slimePrefab.name = "slimeWorm";
            id = slimePrefab.GetComponent<Identifiable>();
            id.id = Ids.WORM_SLIME;
            app = slimePrefab.GetComponent<SlimeAppearanceApplicator>();
            app.SlimeDefinition = def;
            app.Appearance = a;
            GenerateBoneData(app, a.Structures[0].Element.Prefabs[0], 0.3f, 1, null, faceObj, shellObj);
            eat = slimePrefab.GetComponent<SlimeEat>();
            eat.slimeDefinition = def;
            LookupRegistry.RegisterIdentifiablePrefab(id);
            SlimeRegistry.RegisterSlimeDefinition(def);
            #endregion Snom
            //-------------------------------------------------------------------------------------------------------------

            #region Milcery
            def = Object.Instantiate(pinkDef);
            def.AppearancesDefault = new SlimeAppearance[] { Object.Instantiate(def.AppearancesDefault[0]) };
            def.CanLargofy = false;
            def.Diet = new SlimeDiet()
            {
                AdditionalFoods = pinkDef.Diet.AdditionalFoods,
                Favorites = new Identifiable.Id[0],
                FavoriteProductionCount = 2,
                MajorFoodGroups = pinkDef.Diet.MajorFoodGroups,
                Produces = new Identifiable.Id[] { Identifiable.Id.WATER_LIQUID }
            };
            def.FavoriteToys = new Identifiable.Id[0];
            def.IdentifiableId = Ids.CREAM_SLIME;
            def.IsLargo = false;
            def.Name = "Cream Slime";
            def.name = "CreamSlime";
            def.Diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            a = def.AppearancesDefault[0];
            a.ChangeFaceColors(
                Color.white, Color.white, Color.white,
                new Color(0.9f, 0.75f, 0.5f), new Color(0.9f, 0.75f, 0.5f), new Color(0.8f,0.7f,0.6f),
                a.Face.ExpressionFaces.All((x) => true,
                (x) => {
                    var ma = x.Eyes.Clone();
                    ma.SetTextureScale("_texcoord", x.Eyes.GetTextureScale("_texcoord").Multiply(1, 0.5f));
                    ma.SetTextureOffset("_texcoord", x.Eyes.GetTextureOffset("_texcoord") + new Vector2(0, 0.275f));
                    return new SlimeExpressionFace() { Eyes = ma, SlimeExpression = x.SlimeExpression };
                }).ToArray());

            a.Structures = new SlimeAppearanceStructure[] {
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0])
                };
            a.ColorPalette.Ammo = new Color(0.95f, 0.9f, 0.8f);
            a.ColorPalette.Top = new Color(0.95f, 0.9f, 0.8f);
            a.ColorPalette.Middle = new Color(0.95f, 0.9f, 0.8f);
            a.ColorPalette.Bottom = new Color(0.45f, 0.45f, 0.4f);

            a.Structures[0].Element = CreateElement("slimeCreamBase", pinkBodyApp.CreatePrefab());
            skin = a.Structures[0].Element.Prefabs[0].GetComponent<SkinnedMeshRenderer>();
            skin.sharedMesh = Object.Instantiate(skin.sharedMesh);
            skin.sharedMesh.name = "slime_default_rigid_30";
            a.Structures[0].Element.Prefabs[0].name = "slime_default_rigid_30";

            body = a.Structures[0].DefaultMaterials[0].Clone();
            body.name = "slimeCreamBase";
            a.Structures[0].DefaultMaterials[0] = body;
            a.Structures[1].DefaultMaterials[0] = body;
            body.SetColor("_TopColor", new Color(0.95f, 0.9f, 0.8f));
            body.SetColor("_MiddleColor", new Color(0.95f, 0.9f, 0.8f));
            body.SetColor("_BottomColor", new Color(0.45f, 0.45f, 0.4f));
            a.Icon = sprites[Ids.CREAM_SLIME];


            a.Structures[1].Element = CreateElement("slimeCreamTop", pinkBodyApp.CreatePrefab());
            topObj = a.Structures[1].Element.Prefabs[0];
            topObj.name = "slimeCreamTop";
            step = 32;
            step2 = 16;
            ver = new List<Vector3> { new Vector3(0, 0.5f, 0) };
            tri = new List<int>();
            uv = new List<Vector2> { new Vector2(0, 1) };
            for (int j = 0; j < step2; j++)
                tri.AddRange(new int[] { 0, j + 1, (j + 1) % step2 + 1 });
            for (int i = step - 1; i >= 0; i--)
            {
                var y = 1f / step * i;
                for (int j = 0; j < step2; j++)
                    uv.Add(new Vector2(0, y * 0.1f + 0.9f));
                var x = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
                y *= ver[0].y;
                for (int j = 0; j < step2; j++)
                    ver.Add(new Vector3(x * 0.125f, y, 0).Rotate(0, 360f / step2 * j, 0));
                if (i < step - 1)
                    for (int j = 0; j < step2; j++)
                        tri.AddRange(new int[]
                        {
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + j, ver.Count - step2 + (j + 1) % step2,
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + (j + 1) % step2, ver.Count - (step2 * 2) + (j + 1) % step2
                        }
                        );
            }
            for (int i = 0; i < ver.Count; i++)
            {
                var m = 1 - Mathf.Sin((ver[i].y * 2.5f + 0.25f) * Mathf.PI) / 4 - 0.25f;
                ver[i] = ver[i].Multiply(m * 1.5f, 1, m * 1.5f);
            }
            topObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = CreateMesh(ver.ToArray(), tri.ToArray(), uv.ToArray(), new Func<Vector3, Vector3>[] {
                (x) => x.Multiply(1,1.1f,1).Offset(0, 0.8f,0).Rotate(0,0,20).Offset(0, 0.0f,0),
                (x) => x.Offset(0, 0.8f,0).Rotate(0,60,20).Offset(0, 0.0f,0),
                (x) => x.Multiply(1,0.9f,1).Offset(0, 0.8f,0).Rotate(0,120,20).Offset(0, 0.0f,0),
                (x) => x.Offset(0, 0.8f,0).Rotate(0,180,20).Offset(0, 0.0f,0),
                (x) => x.Multiply(1,0.8f,1).Offset(0, 0.8f,0).Rotate(0,-60,20).Offset(0, 0.0f,0),
                (x) => x.Offset(0, 0.8f,0).Rotate(0,-120,20).Offset(0, 0.0f,0)
            }, "slimeCreamTop");


            slimePrefab = GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).CreatePrefab();
            slimePrefab.name = "slimeCream";
            id = slimePrefab.GetComponent<Identifiable>();
            id.id = Ids.CREAM_SLIME;
            app = slimePrefab.GetComponent<SlimeAppearanceApplicator>();
            app.SlimeDefinition = def;
            app.Appearance = a;
            GenerateBoneData(app, a.Structures[0].Element.Prefabs[0], 0.3f, 1, null, topObj, tailObj);
            eat = slimePrefab.GetComponent<SlimeEat>();
            eat.slimeDefinition = def;
            LookupRegistry.RegisterIdentifiablePrefab(id);
            SlimeRegistry.RegisterSlimeDefinition(def);
            #endregion Milcery
            //-------------------------------------------------------------------------------------------------------------

            #region Castform
            def = Object.Instantiate(pinkDef);
            def.AppearancesDefault = new SlimeAppearance[] { Object.Instantiate(def.AppearancesDefault[0]) };
            def.CanLargofy = false;
            def.Diet = new SlimeDiet()
            {
                AdditionalFoods = pinkDef.Diet.AdditionalFoods,
                Favorites = new Identifiable.Id[0],
                FavoriteProductionCount = 2,
                MajorFoodGroups = pinkDef.Diet.MajorFoodGroups,
                Produces = new Identifiable.Id[] { Identifiable.Id.WATER_LIQUID }
            };
            def.FavoriteToys = new Identifiable.Id[0];
            def.IdentifiableId = Ids.WEATHER_SLIME;
            def.IsLargo = false;
            def.Name = "Weather Slime";
            def.name = "WeatherSlime";
            def.Diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            a = def.AppearancesDefault[0];
            a.ChangeFaceColors(
                Color.black, Color.black, Color.black,
                Color.black, Color.black, Color.white.Multiply(0.5f));

            a.Structures = new SlimeAppearanceStructure[] {
                new SlimeAppearanceStructure(a.Structures[0]),
                new SlimeAppearanceStructure(a.Structures[0]),
                //new SlimeAppearanceStructure(a.Structures[0])
                };
            a.ColorPalette.Ammo = new Color(0.8f, 0.8f, 0.9f);
            a.ColorPalette.Top = new Color(0.8f, 0.8f, 0.9f);
            a.ColorPalette.Middle = new Color(0.8f, 0.8f, 0.9f);
            a.ColorPalette.Bottom = new Color(0.75f, 0.75f, 0.8f);


            a.Structures[0].Element = CreateElement("slimeWeatherBase", pinkBodyApp.CreatePrefab());
            skin = a.Structures[0].Element.Prefabs[0].GetComponent<SkinnedMeshRenderer>();
            skin.sharedMesh = Object.Instantiate(skin.sharedMesh);
            skin.sharedMesh.name = "slime_default_rigid_10";
            a.Structures[0].Element.Prefabs[0].name = "slime_default_rigid_10";

            body = a.Structures[0].DefaultMaterials[0].Clone();
            body.name = "slimeWeatherBase";
            a.Structures[0].DefaultMaterials[0] = body;
            //a.Structures[2].DefaultMaterials[0] = body;
            body.SetColor("_TopColor", new Color(0.8f, 0.8f, 0.9f));
            body.SetColor("_MiddleColor", new Color(0.8f, 0.8f, 0.9f));
            body.SetColor("_BottomColor", new Color(0.75f, 0.75f, 0.8f));
            a.Structures[1].DefaultMaterials[0] = body.Clone();
            //a.Structures[1].DefaultMaterials[0].SetColor("_BottomColor", new Color(0.95f, 0.9f, 0.8f));
            a.Icon = sprites[Ids.WEATHER_SLIME];


            a.Structures[1].Element = CreateElement("slimeWeatherTop", pinkBodyApp.CreatePrefab());
            topObj = a.Structures[1].Element.Prefabs[0];
            topObj.name = "slimeWeatherTop";
            step = 32;
            step2 = 8;
            ver = new List<Vector3> { new Vector3(0, 0.5f, 0) };
            tri = new List<int>();
            uv = new List<Vector2> { new Vector2(0, 1) };
            for (int j = 0; j < step2; j++)
                tri.AddRange(new int[] { 0, j + 1, (j + 1) % step2 + 1 });
            for (int i = step - 1; i >= 0; i--)
            {
                var y = 1f / step * i;
                for (int j = 0; j < step2; j++)
                    uv.Add(new Vector2(0, y * 0.2f + 0.8f));
                y *= ver[0].y;
                for (int j = 0; j < step2; j++)
                    ver.Add(new Vector3((ver[0].y - y) * 3f, y, 0).Rotate(0, 360f / step2 * j, 0));
                if (i < step - 1)
                    for (int j = 0; j < step2; j++)
                        tri.AddRange(new int[]
                        {
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + j, ver.Count - step2 + (j + 1) % step2,
                            ver.Count - (step2 * 2) + j, ver.Count - step2 + (j + 1) % step2, ver.Count - (step2 * 2) + (j + 1) % step2
                        }
                        );
            }
            for (int i = 0; i < ver.Count; i++)
            {
                //var m = 1 - Mathf.Sin((ver[i].y * 2.5f + 0.25f) * Mathf.PI) / 4 - 0.25f;
                ver[i] = ver[i].Multiply(0.10f).Rotate(-ver[i].y * 200, 0, 0, 0, 0, -0.5f).Offset(0, 0.8f, 0).Rotate(30, 0, 0, 0, 0.5f, 0).Multiply(1,0.7f,1);
            }
            mesh = new Mesh();
            mesh.name = "slimeWeatherTop";
            mesh.vertices = ver.ToArray();
            mesh.triangles = tri.ToArray();
            mesh.uv = uv.ToArray();
            topObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;


            slimePrefab = GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).CreatePrefab();
            slimePrefab.name = "slimeWeather";
            id = slimePrefab.GetComponent<Identifiable>();
            id.id = Ids.WEATHER_SLIME;
            app = slimePrefab.GetComponent<SlimeAppearanceApplicator>();
            app.SlimeDefinition = def;
            app.Appearance = a;
            GenerateBoneData(app, a.Structures[0].Element.Prefabs[0], 0.1f, 1, null, topObj);
            eat = slimePrefab.GetComponent<SlimeEat>();
            eat.slimeDefinition = def;
            LookupRegistry.RegisterIdentifiablePrefab(id);
            SlimeRegistry.RegisterSlimeDefinition(def);
            #endregion Castform
        }

        public static void Log(string message) => Console.Log($"[{modName}]: " + message);
        public static void LogError(string message) => Console.LogError($"[{modName}]: " + message);
        public static void LogWarning(string message) => Console.LogWarning($"[{modName}]: " + message);
        public static void LogSuccess(string message) => Console.LogSuccess($"[{modName}]: " + message);
        public static Matrix4x4 CreateMatrix(float value)
        {
            var v = new Vector4(value, value, value, value);
            return new Matrix4x4(v, v, v, v);
        }

        static void doT()
        {
            var a = new int[5][];
            a[0] = new int[4];
            a[1] = new int[4];
            a[2] = new int[4];
            a[3] = new int[4];
            a[4] = new int[4];

            var b = new int[5, 4];
        }

        public static Texture2D LoadImage(string filename, int width, int height, TextureWrapMode wrapMode = default)
        {
            var a = modAssembly;
            var spriteData = a.GetManifestResourceStream(a.GetName().Name + "." + filename);
            var rawData = new byte[spriteData.Length];
            spriteData.Read(rawData, 0, rawData.Length);
            var tex = new Texture2D(width, height);
            tex.LoadImage(rawData);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = wrapMode;
            return tex;
        }

        static void BasicSlimeRegistry(Identifiable.Id ident, PediaDirector.Id pedia, Sprite sprit, string name, string intro, string diets, string favou, string ology, string risks, string plort, ZoneDirector.Zone? spawnZone = null)
        {
            SlimeEat.FoodGroup.NONTARRGOLD_SLIMES.AddItem(ident);
            PediaRegistry.RegisterIdEntry(pedia, sprit);
            PediaRegistry.RegisterIdentifiableMapping(pedia, ident);
            PediaRegistry.SetPediaCategory(pedia, PediaRegistry.PediaCategory.SLIMES);
            AmmoRegistry.RegisterPlayerAmmo(PlayerState.AmmoMode.DEFAULT, ident);
            TranslationPatcher.AddActorTranslation("l." + ident.ToString().ToLowerInvariant(), name);
            new SlimePediaEntryTranslation(pedia).SetTitleTranslation(name)
                .SetSlimeologyTranslation(ology)
                .SetPlortonomicsTranslation(plort)
                .SetIntroTranslation(intro)
                .SetFavoriteTranslation(favou)
                .SetRisksTranslation(risks)
                .SetDietTranslation(diets);
            sprites[ident] = sprit;
            //if (spawnZone != null)
                //SpawnArea[ident] = spawnZone.Value;
        }

        public static Mesh CreateMesh(IEnumerable<Vector3> vertices, IEnumerable<int> triangles, IEnumerable<Vector2> uv, Predicate<Vector3> removeAt, Func<Vector3, Vector3> modify, string name = "mesh")
        {
            var m = new Mesh();
            m.name = name;
            var v = vertices.ToList();
            var t = triangles.ToList();
            var u = uv.ToList();
            for (int i = v.Count - 1; i >= 0; i--)
            {
                if (removeAt(v[i]))
                {
                    v.RemoveAt(i);
                    u.RemoveAt(i);
                    for (int j = t.Count - 3; j >= 0; j -= 3)
                    {
                        if (t[j] == i || t[j + 1] == i || t[j + 2] == i)
                        {
                            t.RemoveRange(j, 3);
                            continue;
                        }
                        if (t[j] > i)
                            t[j]--;
                        if (t[j + 1] > i)
                            t[j + 1]--;
                        if (t[j + 2] > i)
                            t[j + 2]--;
                    }
                }
                else
                    v[i] = modify(v[i]);
            }
            m.vertices = v.ToArray();
            m.triangles = t.ToArray();
            m.uv = u.ToArray();
            m.RecalculateBounds();
            m.RecalculateNormals();
            m.RecalculateTangents();
            return m;
        }
        public static Mesh CreateMesh(Vector3[] verticies, int[] triangles, Vector2[] uv, Func<Vector3, Vector3>[] modifiers, string name = "mesh")
        {
            var o = modifiers.Length;
            var v = new Vector3[verticies.Length * o];
            var u = new Vector2[uv.Length * o];
            var t = new int[triangles.Length * o];
            for (int i = 0; i < o; i++)
            {
                for (var j = 0; j < verticies.Length; j++)
                {
                    v[i * verticies.Length + j] = modifiers[i](verticies[j]);
                    u[i * verticies.Length + j] = uv[j];
                }
                for (var j = 0; j < triangles.Length; j++)
                    t[i * triangles.Length + j] = triangles[j] + i * verticies.Length;
            }
            var m = new Mesh();
            m.name = name;
            m.vertices = v;
            m.uv = u;
            m.triangles = t;
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }

        public SlimeAppearanceElement CreateElement(string Name, params SlimeAppearanceObject[] appearanceObjects)
        {
            var e = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
            e.name = Name;
            e.Name = Name;
            e.Prefabs = appearanceObjects;
            return e;
        }

        public static void GenerateBoneData(SlimeAppearanceApplicator slimePrefab, SlimeAppearanceObject bodyApp, float jiggleAmount = 1, float scale = 1, Mesh[] AdditionalMesh = null, params SlimeAppearanceObject[] appearanceObjects)
        {
            if (AdditionalMesh == null)
                AdditionalMesh = new Mesh[0];
            var mesh = bodyApp.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            bodyApp.AttachedBones = new SlimeAppearance.SlimeBone[] { SlimeAppearance.SlimeBone.Slime, SlimeAppearance.SlimeBone.JiggleRight, SlimeAppearance.SlimeBone.JiggleLeft, SlimeAppearance.SlimeBone.JiggleTop, SlimeAppearance.SlimeBone.JiggleBottom, SlimeAppearance.SlimeBone.JiggleFront, SlimeAppearance.SlimeBone.JiggleBack };
            foreach (var a in appearanceObjects)
                a.AttachedBones = bodyApp.AttachedBones;
            var v = mesh.vertices;
            var max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            var min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            var sum = Vector3.zero;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i];
                if (v[i].x > max.x)
                    max.x = v[i].x;
                if (v[i].x < min.x)
                    min.x = v[i].x;
                if (v[i].y > max.y)
                    max.y = v[i].y;
                if (v[i].y < min.y)
                    min.y = v[i].y;
                if (v[i].z > max.z)
                    max.z = v[i].z;
                if (v[i].z < min.z)
                    min.z = v[i].z;
            }
            var center = sum / v.Length;
            var dis = 0f;
            foreach (var ver in v)
                dis += (ver - center).magnitude;
            dis /= v.Length;

            foreach (var m in new Mesh[] { mesh }.Concat(appearanceObjects.All((x) => true,(x) => x.GetComponent<SkinnedMeshRenderer>().sharedMesh)).Concat(AdditionalMesh)) {
                Log(m.name);
                var v2 = m.vertices;
                var b = new BoneWeight[v2.Length];
                for (int i = 0; i < v2.Length; i++)
                {
                    var r = v2[i] - center;
                    var o = Mathf.Clamp01((r.magnitude - (dis / 4)) / (dis / 2) * jiggleAmount);
                    b[i] = new BoneWeight();
                    if (o == 0)
                        b[i].weight0 = 1;
                    else
                    {
                        b[i].weight0 = 1 - o;
                        b[i].boneIndex1 = r.x >= 0 ? 1 : 2;
                        b[i].boneIndex2 = r.y >= 0 ? 3 : 4;
                        b[i].boneIndex3 = r.z >= 0 ? 5 : 6;
                        var n = r.Multiply(r).Multiply(r).Abs();
                        var s = n.ToArray().Sum();
                        b[i].weight1 = n.x / s * o;
                        b[i].weight2 = n.y / s * o;
                        b[i].weight3 = n.z / s * o;
                    }
                    b[i].weight0 *= scale;
                    b[i].weight1 *= scale;
                    b[i].weight2 *= scale;
                    b[i].weight3 *= scale;
                }
                m.boneWeights = b;

                var p = new Matrix4x4[bodyApp.AttachedBones.Length];
                for (int i = 0; i < bodyApp.AttachedBones.Length; i++)
                    p[i] = slimePrefab.Bones.First((x) => x.Bone == bodyApp.AttachedBones[i]).BoneObject.transform.worldToLocalMatrix * slimePrefab.Bones.First((x) => x.Bone == SlimeAppearance.SlimeBone.Root).BoneObject.transform.localToWorldMatrix;
                m.bindposes = p;
            }
        }
    }

    [EnumHolder]
    static class Ids // Castform, Pincurchin
    {
        public static Identifiable.Id STOMACH_SLIME;
        public static Identifiable.Id SEA_CUCUMBER_SLIME;
        public static Identifiable.Id SOFT_TISSUE_SLIME;
        public static Identifiable.Id WORM_SLIME;
        public static Identifiable.Id CREAM_SLIME;
        public static Identifiable.Id WEATHER_SLIME;
    }

    [EnumHolder]
    static class Ids2
    {
        public static PediaDirector.Id STOMACH_SLIME;
        public static PediaDirector.Id SEA_CUCUMBER_SLIME;
        public static PediaDirector.Id SOFT_TISSUE_SLIME;
        public static PediaDirector.Id WORM_SLIME;
        public static PediaDirector.Id CREAM_SLIME;
        public static PediaDirector.Id WEATHER_SLIME;
    }

    static class ExtentionMethods
    {
        public static Sprite CreateSprite(this Texture2D texture) => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);

        public static Material Clone(this Material material)
        {
            var m = new Material(material);
            m.CopyPropertiesFromMaterial(material);
            return m;
        }
        public static string RemoveAfterLast(this string value, string delimeter) => !string.IsNullOrEmpty(delimeter) && !string.IsNullOrEmpty(value) && value.Contains(delimeter) ? value.Remove(value.LastIndexOf(delimeter)) : value;
        public static string RemoveBefore(this string value, string delimeter) => !string.IsNullOrEmpty(delimeter) && !string.IsNullOrEmpty(value) && value.Contains(delimeter) ? value.Remove(0, value.IndexOf(delimeter) + delimeter.Length) : value;

        public static string GetSuffix(this IEnumerable<string> a)
        {
            var m = a.Min((x) => x.Length);
            var c = "";
            while (c.Length < m)
            {
                var e = a.First().Remove(0, a.First().Length - c.Length - 1);
                if (a.All((x) => e == x.Remove(0, x.Length - c.Length - 1)))
                    c = e;
                else
                    break;
            }
            return c;
        }
        public static string GetPrefix(this IEnumerable<string> a)
        {
            var m = a.Min((x) => x.Length);
            var c = "";
            while (c.Length < m)
            {
                var e = a.First().Remove(c.Length + 1);
                if (a.All((x) => e == x.Remove(c.Length + 1)))
                    c = e;
                else
                    break;
            }
            return c;
        }
        public static string GetName(this Dictionary<string, string> c, Identifiable.Id id)
        {
            if (c.TryGetValue("l." + id.ToString().ToLowerInvariant(), out var r))
                return r;
            Main.LogWarning("Failed to find name for " + id);
            return "???";
        }

        public static void CopyFields<T>(this T a, T b)
        {
            var t = typeof(T);
            while (t != typeof(Object) && t != typeof(object))
            {
                foreach (var f in t.GetFields((BindingFlags)(-1)))
                    if (!f.IsStatic)
                        f.SetValue(a, f.GetValue(b));
                t = t.BaseType;
            }
        }

        public static Sprite GetReadable(this Sprite source)
        {
            return Sprite.Create(source.texture.GetReadable(), source.rect, source.pivot, source.pixelsPerUnit);
        }

        public static Texture2D GetReadable(this Texture2D source)
        {
            Texture2D texture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, source.mipmapCount, true);
            Graphics.CopyTexture(source, texture);
            return texture;
        }

        public static Cubemap GetReadable(this Cubemap source)
        {
            Cubemap texture = new Cubemap(source.width, TextureFormat.RGBA32, source.mipmapCount);
            Graphics.CopyTexture(source, texture);
            return texture;
        }

        public static void ModifyTexturePixels(this Texture2D texture, Func<Color, Color> colorChange)
        {
            for (int m = 0; m < texture.mipmapCount; m++)
            {
                var p = texture.GetPixels(m);
                for (int x = 0; x < p.Length; x++)
                    p[x] = colorChange(p[x]);
                texture.SetPixels(p, m);
            }
            texture.Apply(true);
        }

        public static void ModifyTexturePixels(this Texture2D texture, Func<Color, float, float, Color> colorChange)
        {
            for (int m = 0; m < texture.mipmapCount; m++)
            {
                var p = texture.GetPixels(m);
                for (int x = 0; x < p.Length; x++)
                    p[x] = colorChange(p[x], (x % texture.width + 1) / (float)texture.width, (x / texture.width + 1) / (float)texture.height);
                texture.SetPixels(p, m);
            }
            texture.Apply(true);
        }

        public static void ModifyTexturePixels(this Cubemap texture, Func<Color, Color> colorChange)
        {
            var p = new Color[texture.mipmapCount][][];
            for (int m = 0; m < texture.mipmapCount; m++)
            {
                p[m] = new Color[6][];
                for (CubemapFace f = 0; f <= CubemapFace.NegativeZ; f++)
                    p[m][(int)f] = texture.GetPixels(f, 0);
            }
            for (int m = 0; m < texture.mipmapCount; m++)
                for (CubemapFace f = 0; f <= CubemapFace.NegativeZ; f++)
                {
                    for (int x = 0; x < p[m][(int)f].Length; x++)
                        p[m][(int)f][x] = colorChange(p[m][(int)f][x]);
                    texture.SetPixels(p[m][(int)f], f, m);
                }
            texture.Apply();
        }

        public static bool Exists<T>(this IEnumerable<T> c, Predicate<T> predicate)
        {
            foreach (var i in c)
                if (predicate(i))
                    return true;
            return false;
        }

        public static List<Y> All<X, Y>(this IEnumerable<X> c, Predicate<X> predicate, Func<X, Y> converter, bool enforceUnique = false)
        {
            var l = new List<Y>();
            foreach (var i in c)
                if (predicate(i))
                {
                    if (enforceUnique)
                        l.AddUnique(converter(i));
                    else
                        l.Add(converter(i));
                }
            return l;
        }

        public static List<Y> All<X, Y>(this IEnumerable<X> c, Predicate<X> predicate, Func<X, IEnumerable<Y>> converter, bool enforceUnique = false)
        {
            var l = new List<Y>();
            foreach (var i in c)
                if (predicate(i))
                {
                    if (enforceUnique)
                        l.AddRangeUnique(converter(i));
                    else
                        l.AddRange(converter(i));
                }
            return l;
        }

        public static bool AddUnique<T>(this List<T> l, T value)
        {
            if (l.Contains(value))
                return false;
            l.Add(value);
            return true;
        }

        public static void AddRangeUnique<T>(this List<T> l, IEnumerable<T> values)
        {
            foreach (var v in values)
                l.AddUnique(v);
        }

        public static T CreatePrefab<T>(this T obj) where T : Object => Object.Instantiate(obj, Main.prefabParent, false);

        public static T CreateInactive<T>(this T obj) where T : Component
        {
            var o = Object.Instantiate(obj, Main.prefabParent, true);
            o.gameObject.SetActive(false);
            o.transform.SetParent(null, true);
            return o;
        }
        public static GameObject CreateInactive(this GameObject obj)
        {
            var o = Object.Instantiate(obj, Main.prefabParent, true);
            o.SetActive(false);
            o.transform.SetParent(null, true);
            return o;
        }

        public static bool Matches(this SlimeDefinition a, SlimeDefinition b) => a == b || (a.BaseSlimes != null && a.BaseSlimes.Contains(b)) || (b.BaseSlimes != null && b.BaseSlimes.Contains(a));
        public static bool Matches(this SlimeDefinition a, Identifiable.Id id) => a.IdentifiableId == id || (a.BaseSlimes != null && a.BaseSlimes.Exists((x) => x.IdentifiableId == id));

        public static Color Multiply(this Color color, float r, float g, float b) => new Color(color.r * r, color.g * g, color.b * b, color.a);
        public static Color Multiply(this Color color, float m) => new Color(color.r * m, color.g * m, color.b * m, color.a);
        public static Color Shift(this Color color, float r, float g, float b)
        {
            var v = color.grayscale;
            return new Color(v * r, v * g, v * b, color.a);
        }

        public static float Unhappiness(this SlimeEmotions emotions) => Mathf.Max(emotions.GetCurr(SlimeEmotions.Emotion.AGITATION), emotions.GetCurr(SlimeEmotions.Emotion.HUNGER), emotions.GetCurr(SlimeEmotions.Emotion.FEAR));

        public static void AddItem(this SlimeEat.FoodGroup foodGroup, Identifiable.Id ident) => SlimeEat.foodGroupIds[foodGroup] = SlimeEat.foodGroupIds.TryGetValue(foodGroup, out var v) ? v.AddToArray(ident) : new Identifiable.Id[] { ident };

        public static T RandomObject<T>(this IEnumerable<T> c, Func<T, float> getWeight)
        {
            var r = Random.Range(0, c.Sum(getWeight));
            foreach (var i in c)
            {
                r -= getWeight(i);
                if (r < 0)
                    return i;
            }
            return default;
        }

        public static Vector3 Rotate(this Vector3 value, Quaternion rotation) => rotation * value;
        public static Vector3 Rotate(this Vector3 value, Vector3 rotation) => value.Rotate(Quaternion.Euler(rotation));
        public static Vector3 Rotate(this Vector3 value, float x, float y, float z) => value.Rotate(Quaternion.Euler(x, y, z));
        public static Vector3 Rotate(this Vector3 value, Quaternion rotation, Vector3 rotatePoint) => value.Offset(-rotatePoint).Rotate(rotation).Offset(rotatePoint);
        public static Vector3 Rotate(this Vector3 value, Vector3 rotation, Vector3 rotatePoint) => value.Rotate(Quaternion.Euler(rotation), rotatePoint);
        public static Vector3 Rotate(this Vector3 value, float x, float y, float z, Vector3 rotatePoint) => value.Rotate(Quaternion.Euler(x, y, z), rotatePoint);
        public static Vector3 Rotate(this Vector3 value, Quaternion rotation, float rotatePointX, float rotatePointY, float rotatePointZ) => value.Rotate(rotation, new Vector3(rotatePointX, rotatePointY, rotatePointZ));
        public static Vector3 Rotate(this Vector3 value, Vector3 rotation, float rotatePointX, float rotatePointY, float rotatePointZ) => value.Rotate(Quaternion.Euler(rotation), new Vector3(rotatePointX, rotatePointY, rotatePointZ));
        public static Vector3 Rotate(this Vector3 value, float x, float y, float z, float rotatePointX, float rotatePointY, float rotatePointZ) => value.Rotate(Quaternion.Euler(x, y, z), new Vector3(rotatePointX, rotatePointY, rotatePointZ));
        public static Vector3 Offset(this Vector3 value, float x, float y, float z) => value.Offset(new Vector3(x, y, z));
        public static Vector3 Offset(this Vector3 value, Vector3 offset) => value + offset;
        public static Vector3 Multiply(this Vector3 value, float x, float y, float z) => new Vector3(value.x * x, value.y * y, value.z * z);
        public static Vector3 Multiply(this Vector3 value, float scale) => value.Multiply(scale, scale, scale);
        public static Vector3 Multiply(this Vector3 value, Vector3 scale) => value.Multiply(scale.x, scale.y, scale.z);
        public static Vector2 Multiply(this Vector2 value, float x, float y) => new Vector2(value.x * x, value.y * y);
        public static Vector2 Multiply(this Vector2 value, Vector3 scale) => value.Multiply(scale.x, scale.y);
        public static float[] ToArray(this Vector3 value) => new float[] { value.x, value.y, value.z };
        public static float Min(this Vector3 value) => Mathf.Min(value.ToArray());
        public static float Max(this Vector3 value) => Mathf.Max(value.ToArray());
        public static Vector3 Abs(this Vector3 value) => new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));

        public static List<Y> GetValues<X,Y>(this Dictionary<X,Y> d, IEnumerable<X> keys)
        {
            var r = new List<Y>();
            foreach (var key in keys)
                if (d.TryGetValue(key, out var value))
                    r.Add(value);
                else
                    r.Add(default);
            return r;
        }
        public static void AddRange<X, Y>(this Dictionary<X, Y> d, IEnumerable<X> keys, IEnumerable<Y> values)
        {
            var k = keys.GetEnumerator();
            var v = values.GetEnumerator();
            while (k.MoveNext() && v.MoveNext())
                d.Add(k.Current, v.Current);
        }

        public static void ChangeFaceColors(this SlimeAppearance a, Color eye1, Color eye2, Color eye3, Color mouth1, Color mouth2, Color mouth3, params SlimeExpressionFace[] replacements)
        {
            a.Face = Object.Instantiate(a.Face);
            var faces = a.Face.ExpressionFaces;
            var rep = new Dictionary<Material, Material>();
            for (int i = 0; i < faces.Length; i++)
            {
                var replace = replacements.FirstOrDefault((x) => x.SlimeExpression == faces[i].SlimeExpression);
                if (replace.Eyes)
                    faces[i].Eyes = replace.Eyes;
                if (replace.Mouth)
                    faces[i].Mouth = replace.Mouth;
                if (faces[i].Eyes)
                {
                    if (!rep.TryGetValue(faces[i].Eyes, out var eye))
                    {
                        eye = faces[i].Eyes.Clone();
                        eye.name = eye.name.Replace("(Clone)", "").Replace(" (Instance)", "") + "Stomach";
                        if (eye.HasProperty("_EyeRed"))
                            eye.SetColor("_EyeRed", eye1);
                        if (eye.HasProperty("_EyeGreen"))
                            eye.SetColor("_EyeGreen", eye2);
                        if (eye.HasProperty("_EyeBlue"))
                            eye.SetColor("_EyeBlue", eye3);
                        rep.Add(faces[i].Eyes, eye);
                    }
                    faces[i].Eyes = eye;
                }
                if (faces[i].Mouth)
                {
                    if (!rep.TryGetValue(faces[i].Mouth, out var mouth))
                    {
                        mouth = faces[i].Mouth.Clone();
                        mouth.name = mouth.name.Replace("(Clone)", "").Replace(" (Instance)", "") + "Stomach";
                        if (mouth.HasProperty("_MouthTop"))
                            mouth.SetColor("_MouthTop", mouth1);
                        if (mouth.HasProperty("_MouthMid"))
                            mouth.SetColor("_MouthMid", mouth2);
                        if (mouth.HasProperty("_MouthBot"))
                            mouth.SetColor("_MouthBot", mouth3);
                        rep.Add(faces[i].Mouth, mouth);
                    }
                    faces[i].Mouth = mouth;
                }
            }
            a.Face.OnEnable();
        }
    }
}