using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace NHDServer
{
    class Player
    {
        public int id;
        public string username;
        public Vector3 position;
        public Quaternion rotation;

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;
        }

        public void Update()
        {
            Move(position);
        }

        private void Move(Vector3 _nextPosition)
        {
            ServerSend.PlayerPosition(this);
        }

        public void SetPosition(Vector3 _position, Quaternion _rotation)
        {
            position = _position;
            rotation = _rotation;
        }
    }
}
