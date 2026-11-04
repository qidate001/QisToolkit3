using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.SCG.Scenes
{
    /// <summary>
    /// 场景基类。所有页面都继承它。
    /// 通过 SceneRegistry 注册后，Form 就不再需要 switch 分支。
    /// </summary>
    public abstract class SceneBase
    {
        protected readonly SurvivalChallengeGame Form;

        protected SceneBase(SurvivalChallengeGame form) => Form = form;

        /// <summary>该场景对应的枚举值（唯一标识）。</summary>
        public abstract SceneId Id { get; }

        /// <summary>进入场景时调用：设置文字、按钮文本、按钮可用性。</summary>
        public abstract void Enter();

        /// <summary>玩家点了 0~4 号按钮之一。</summary>
        public abstract void HandleOption(int index);

        /// <summary>点"下一页"。无分页的场景不用重写。</summary>
        public virtual void OnNext() { }

        /// <summary>点"上一页"。无分页的场景不用重写。</summary>
        public virtual void OnBack() { }
    }
}
