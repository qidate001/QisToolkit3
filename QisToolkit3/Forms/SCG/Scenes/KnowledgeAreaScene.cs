using System;
using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class KnowledgeAreaScene : SceneBase
    {
        public KnowledgeAreaScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.KnowledgeArea;

        public override void Enter()
        {
            Form.XSetMessage("你看向了“知识区”，你打算前往……");
            Form.SetButtonTexts("权杖：图书馆", "面具：艺术馆", "书本：历史馆", "骰子：占卜馆", "镰刀：消逝馆");
            Form.SetButtonEnableds(
                Item_God == 0 || Item_God == 4,
                Item_God == 0 || Item_God == 1 || Item_God == 2,
                Item_God == 0 || Item_God == 5,
                Item_God == 0 || Item_God == 2 || Item_God == 1,
                Item_God == 0 || Item_God == 3
            );
        }

        public override void HandleOption(int index)
        {
            int Tmp;
            switch (index)
            {
                // 图书馆（秩序）
                case 0:
                    if (Item_God == 0)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            Form.XSetMessage("你前往了图书馆，你在里面看了一上午，沉浸在知识的海洋", 0, 0, Tmp);
                        else
                            Form.XSetMessage("你前往了图书馆，你在里面看了一下午，沉浸在知识的海洋", 0, 0, Tmp);
                    }
                    else if (Item_God == 4)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Item_DarkBlocker)
                        {
                            if (Morning)
                                Form.XSetMessage("你前往了图书馆，你在里面看了一上午，享受着秩序的光辉", 0, 0, Tmp + 10);
                            else
                                Form.XSetMessage("你前往了图书馆，你在里面看了一下午，享受着秩序的光辉", 0, 0, Tmp + 10);
                        }
                        else
                        {
                            if (Morning)
                                Form.XSetMessage("你前往了图书馆，你在里面看了一上午，享受着██的光辉", 0, 0, Tmp + 10);
                            else
                                Form.XSetMessage("你前往了图书馆，你在里面看了一下午，享受着██的光辉", 0, 0, Tmp + 10);
                        }
                    }
                    else
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Item_DarkBlocker)
                        {
                            if (Morning)
                                Form.XSetMessage("你前往了图书馆，你在里面看了一上午，但同样秩序的存在让你很不舒服", 0, 0, Tmp - 10);
                            else
                                Form.XSetMessage("你前往了图书馆，你在里面看了一下午，但同样秩序的存在让你很不舒服", 0, 0, Tmp - 10);
                        }
                        else
                        {
                            if (Morning)
                                Form.XSetMessage("你前往了图书馆，你在里面看了一上午，但同样██的存在让你很不舒服", 0, 0, Tmp - 10);
                            else
                                Form.XSetMessage("你前往了图书馆，你在里面看了一下午，但同样██的存在让你很不舒服", 0, 0, Tmp - 10);
                        }
                    }
                    break;

                // 艺术馆（欺诈）
                case 1:
                    if (Item_God == 0)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            Form.XSetMessage("你前往了艺术馆，你在里面看了一上午，沉浸在艺术之美中", 0, 0, Tmp);
                        else
                            Form.XSetMessage("你前往了艺术馆，你在里面看了一下午，沉浸在艺术之美中", 0, 0, Tmp);
                    }
                    else if (Item_God == 1 || Item_God == 2)
                    {
                        Tmp = new Random().Next(1, 50);
                        if (Item_DarkBlocker)
                            Form.XSetMessage("你前往了艺术馆，你仿佛进入了那传说中的嬉笑之潮。\n在那更深层的虚空之中，无数的“镜子”呈现出无尽的抽象", 0, 0, Tmp);
                        else
                            Form.XSetMessage("你前往了艺术馆，你仿佛进入了那传说中的████。\n在那更深层的██之中，无数的“██”呈现出█████", 0, 0, Tmp);
                    }
                    else
                    {
                        Tmp = new Random().Next(-15, 15);
                        if (Item_DarkBlocker)
                        {
                            if (Morning)
                                Form.XSetMessage("你前往了艺术馆，你在里面看了一上午，你仿佛感受到无数双嬉笑的面孔正盯着你", 0, 0, Tmp);
                            else
                                Form.XSetMessage("你前往了艺术馆，你在里面看了一下午，你仿佛感受到无数双嬉笑的面孔正盯着你", 0, 0, Tmp);
                        }
                        else
                        {
                            if (Morning)
                                Form.XSetMessage("你前往了艺术馆，你在里面看了一上午，你仿佛感受到████████正盯着你", 0, 0, Tmp);
                            else
                                Form.XSetMessage("你前往了艺术馆，你在里面看了一下午，你仿佛感受到████████正盯着你", 0, 0, Tmp);
                        }
                    }
                    break;

                // 历史馆（记忆）
                case 2:
                    if (Item_God == 0)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            Form.XSetMessage("你前往了历史馆，你在里面看了一上午，沉浸在对历史的惊叹之中", 0, 0, Tmp);
                        else
                            Form.XSetMessage("你前往了历史馆，你在里面看了一下午，沉浸在对历史的惊叹之中", 0, 0, Tmp);
                    }
                    else if (Item_God == 5)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Item_DarkBlocker)
                            Form.XSetMessage("你前往了历史馆，你仿佛进入了那传说中的记忆神殿。\n在那被时间长河与嬉笑之潮包围，充满无量知识的神圣大殿", 0, 0, Tmp + 10);
                        else
                            Form.XSetMessage("你前往了历史馆，你仿佛进入了那传说中的████。\n在那被████与████包围，充满████的████", 0, 0, Tmp + 10);
                    }
                    else
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            Form.XSetMessage("你前往了历史馆，你在里面看了一上午，你仿佛感受到那些历史活了过来，正盯着你", 0, 0, Tmp - 10);
                        else
                            Form.XSetMessage("你前往了历史馆，你在里面看了一下午，你仿佛感受到那些历史活了过来，正盯着你", 0, 0, Tmp - 10);
                    }
                    break;

                // 占卜馆（命运）
                case 3:
                    if (Item_God != 1 && Item_God != 2)
                    {
                        string StrTmp = "暂无预言";
                        if (Days < 10) StrTmp = (10 - Days) + " 天之后世界将大变。";
                        else if (Days >= 10 && Days < ExtremeColdComes) StrTmp = "天启之相，伴 " + (ExtremeColdComes - Days) + " 日后极寒降临。";
                        else if (Days >= EndOfExtremeCold && Days < ExtremeHotComes) StrTmp = "戮地之相，伴 " + (ExtremeHotComes - Days) + " 日后极热降临。";

                        if (Morning)
                            Form.XSetMessage("你前往了占卜馆，上面写着：“" + StrTmp + "”\n当你回过神之时，上午已经过去了", 0, 0, 1);
                        else
                            Form.XSetMessage("你前往了占卜馆，上面写着：“" + StrTmp + "”\n当你回过神之时，下午已经过去了", 0, 0, 1);
                    }
                    else if (Item_God == 1 || Item_God == 2)
                    {
                        Tmp = new Random().Next(15, 45);
                        if (Item_DarkBlocker)
                            Form.XSetMessage("你前往了占卜馆，你仿佛看到了那传说中的命运神轮。\n祂坐在神轮前，执掌着万千生命的命与运", 0, 0, Tmp + 10);
                        else
                            Form.XSetMessage("你前往了占卜馆，你仿佛看到了那传说中的████。\n█████前，██着████的█与█", 0, 0, Tmp + 10);
                    }
                    else
                    {
                        if (Item_DarkBlocker)
                            Form.XSetMessage("你前往了占卜馆，但很明显祂不想见你，上来就给你了一个闭门羹", 0, 0, -1, -10);
                        else
                            Form.XSetMessage("你前往了占卜馆，但很明显█不想见你，上来就给你了一个闭门羹", 0, 0, -1, -10);
                    }
                    break;

                // 消逝馆（死亡）
                case 4:
                    if (Item_God == 0)
                    {
                        if (Morning)
                            Form.XSetMessage("你前往了消逝馆，你在里面看了一上午，懵了一上午", 0, 0, 5);
                        else
                            Form.XSetMessage("你前往了消逝馆，你在里面看了一下午，懵了一下午", 0, 0, 5);
                    }
                    else if (Item_God == 3)
                    {
                        if (Item_DarkBlocker)
                            Form.XSetMessage("你前往了消逝馆，你仿佛踏上了那传说中的死亡骨阶。\n在那不可承载一切生命，你仿佛看到自己就是一副骨架，踏上面见祂的骨阶", 0, 0, 15);
                        else
                            Form.XSetMessage("你前往了消逝馆，你仿佛踏上了那传说中的████。\n在那不可██████，你仿佛看到自己就是████，踏上面见█的██", 0, 0, 15);
                    }
                    else
                    {
                        Tmp = new Random().Next(-10, 2);
                        if (Morning)
                            Form.XSetMessage("你前往了历史馆，你在里面看了一上午，在那里充斥着浓郁的死亡气息，令你汗毛直竖", 0, 0, Tmp);
                        else
                            Form.XSetMessage("你前往了历史馆，你在里面看了一下午，在那里充斥着浓郁的死亡气息，令你汗毛直竖", 0, 0, Tmp);
                    }
                    break;
            }

            KnowledgeCd = Item_God == 0 ? 1 : 2;

            Form.EnterScene(SceneId.Main);
        }
    }
}