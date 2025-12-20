using OutwardBasicChatCommands.Utility.Data;
using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Managers
{
    public class FollowerDataManager 
    {
        private static FollowerDataManager _instance;

        private FollowerDataManager()
        {
        }

        public static FollowerDataManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FollowerDataManager();

                return _instance;
            }
        }

        private Dictionary<UID, FollowerData> _followerData = new();

        public Dictionary<UID, FollowerData> FollowerData { get => _followerData; set => _followerData = value; }

        public bool IsCharacterFollowing(Character follower, out FollowerData data)
        {
            if (!FollowerData.TryGetValue(follower.UID, out data))
                return false;

            if (!data.IsFollowing)
                return false;

            return true;
        }

        public void TryToFollow(Character follower, Character charToFollow)
        {
            if(string.Equals(follower.UID, charToFollow.UID))
            {
                ChatHelpers.SendChatLog(follower, "You cannot follow yourself! Try other target.", ChatLogStatus.Warning);
                return;
            }

            FollowerData data = null;

            if (!FollowerData.TryGetValue(follower.UID, out data))
            {
                data = new FollowerData(follower, charToFollow);
                FollowerData.Add(follower.UID, data);
            }
            else
            {
                if(data.CharacterToFollow.UID != charToFollow.UID)
                {
                    data.CharacterToFollow = charToFollow;
                }
            }

            data.StartFollow();
        }

        public void RemoveFollower(UID uid)
        {
            FollowerData.Remove(uid);
        }
    }
}
