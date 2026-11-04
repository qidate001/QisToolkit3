using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.SCG.Scenes
{
    /// <summary>
    /// 场景注册表。Form 构造时把所有场景 Register 进来，运行时按 SceneId 查找。
    /// </summary>
    public static class SceneRegistry
    {
        private static readonly Dictionary<SceneId, SceneBase> _map = new();

        public static void Register(SceneBase scene, params SceneId[] extraIds)
        {
            _map[scene.Id] = scene;
            foreach (var id in extraIds)
                _map[id] = scene;
        }

        public static bool TryGet(SceneId id, out SceneBase scene) => _map.TryGetValue(id, out scene);
    }
}
