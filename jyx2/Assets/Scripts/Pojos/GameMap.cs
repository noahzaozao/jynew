/*
 * 金庸群侠传3D重制版
 * https://github.com/jynew/jynew
 *
 * 这是本开源项目文件头，所有代码均使用MIT协议。
 * 但游戏内资源和第三方插件、dll等请仔细阅读LICENSE相关授权协议文档。
 *
 * 金庸老先生千古！
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HSFrameWork.ConfigTable;
using Jyx2;

namespace Jyx2
{
    [XmlType("gamemap")]
    public class GameMap : BaseBean
    {
        public override string PK { get { return Key; } }

        [XmlAttribute]
        public string Key;

        [XmlAttribute]
        public string BigMapTriggerName;

        [XmlAttribute]
        public string Jyx2MapId;

        [XmlAttribute]
        public string Name;
        
		// display XiaoxiamiJu with player namespace
		//modified by eaphone at 2021/05/22
        public string GetShowName()
        {
			var result=Name;
            if (!string.IsNullOrEmpty(Jyx2MapId))
            {
                result=ConfigTable.Get<Jyx2Map>(Jyx2MapId).Name;
            }
			if ("小虾米居".Equals(result)) result=GameRuntimeData.Instance.Player.Name+"居";

            return result;
        }

        [XmlAttribute]
        public string Tags;

		/// <summary>
		/// 当前是否在大地图，统一判断方式
		/// </summary>
		public bool IsWorldMap { get { return m_isWorldMap; } }

		public override void InitBind()
		{
			base.InitBind();

            m_isWorldMap = Tags.Contains("WORLDMAP");
        }

		//获得开场地图
		public static GameMap GetGameStartMap()
        {
            foreach(var map in ConfigTable.GetAll<GameMap>())
            {
                if (map.Tags.Contains("START"))
                {
                    return map;
                }
            }
            return null;
        }

        public string GetEnterMusic()
        {
            if(!string.IsNullOrEmpty(Jyx2MapId))
            {
                var map = ConfigTable.Get<Jyx2Map>(Jyx2MapId);

                if (map == null || map.InMusic < 0)
                    return string.Empty;

                string path = "Assets/BuildSource/Musics/" + (map.InMusic) + ".mp3";
                return path;
            }
            else
            {
                return string.Empty;
            }
        }

        //强制设置离开音乐
        public int ForceSetLeaveMusicId = -1;

        public string GetLeaveMusic()
        {
            //强制设置的离开音乐
            if(ForceSetLeaveMusicId != -1)
            {
                return "Assets/BuildSource/Musics/" + ForceSetLeaveMusicId + ".mp3";
            }

            if (!string.IsNullOrEmpty(Jyx2MapId))
            {
                var map = ConfigTable.Get<Jyx2Map>(Jyx2MapId);
                if (map == null || map.OutMusic < 0)
                    return string.Empty;

                string path = "Assets/BuildSource/Musics/" + (map.OutMusic) + ".mp3";
                return path;
            }
            else
            {
                return string.Empty;
            }
        }

        bool m_isWorldMap = false;
    }
}
