using System;


namespace Assets.Source.Core.Components.Lobby
{
    public class GameRoomMetaData
    {
        public GameType p_type;
    }


    [Serializable]
    public enum GameType { 
        TeamCoop,
        Deathmatch,
    }
}
