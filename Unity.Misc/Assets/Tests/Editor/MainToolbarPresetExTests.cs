using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace xpTURN.Misc.Tests
{
    public class MainToolbarPresetExTests
    {
        const string k_PresetPath = "Assets/Settings/ToolbarPreset.asset";
        const string k_VersionKey = "MainToolbarPresetEx.AppliedVersion";

        [Test]
        public void Reset_DeletesAppliedVersionKey()
        {
            EditorPrefs.SetInt(k_VersionKey, 99);
            MainToolbarPresetEx.Reset();
            Assert.That(EditorPrefs.GetInt(k_VersionKey, -1), Is.EqualTo(-1), "Version key should be deleted or default.");
        }

        [Test]
        public void ApplyToolbarDefault_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => MainToolbarPresetEx.ApplyToolbarDefault());
        }

        [Test]
        public void ApplyToolbarPreset_NoArg_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => MainToolbarPresetEx.ApplyToolbarPreset());
        }

        [Test]
        public void ApplyToolbarPreset_WithPreset_DoesNotThrow()
        {
            var preset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(k_PresetPath);
            if (preset == null)
            {
                Assert.Ignore($"Toolbar preset not found at: {k_PresetPath}");
                return;
            }

            Assert.DoesNotThrow(() => MainToolbarPresetEx.ApplyToolbarPreset(preset));
            // 프로젝트 에셋은 Destroy 하지 않음 (데이터 손실 방지)
        }
    }
}
