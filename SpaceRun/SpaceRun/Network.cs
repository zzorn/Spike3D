using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceRun
{
    class Placeholder_PlayerShip
    {
        public int id;
        public float health;
        public Vector3 position;
        public Quaternion heading; //rotational state
        public Vector3 velocity;
        public Vector3 rotation; //angular movement

        public Placeholder_PlayerShip(int a_id)
        {
            id = a_id;
        }
    }

    class Network
    {
        #region haxorz

        public Placeholder_PlayerShip[] playerShips = new Placeholder_PlayerShip[2];

        private enum EntityEventType { State, Remove }
        private enum GamePacketType { Connect, Disconnect, Ship, Asteroid }

        #endregion

        NetworkSession session;
        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        string networkErrorMessage = "";

        int maxLocalPlayers = 1;
        int maxNetworkPlayers = 8;

        /// <summary>
        /// Updates the state of the network session, moving the players
        /// around and synchronizing their state over the network.
        /// </summary>
        protected void Updatesession()
        {
            // Make sure the session has not ended.
            if (session == null)
                return;

            // Update our locally controlled tanks, and send their
            // latest position data to everyone in the session.
            foreach (LocalNetworkGamer gamer in session.LocalGamers)
            {
                UpdateLocalGamer(gamer);
            }

            // Pump the underlying session object.
            session.Update();

            // Read any packets telling us the position of remotely controlled player.
            foreach (LocalNetworkGamer gamer in session.LocalGamers)
            {
                ReadIncomingPackets(gamer);
            }
        }

        private void UpdateLocalGamer(LocalNetworkGamer gamer)
        {
            //Check if the gamer is host and write appropriate network packets
            /*if (player[1].playerType == LocalPlayer)
            {
                packetWriter.Write(gameState);
                packetWriter.Write(player[1].alive);
                packetWriter.Write(player[1].playerReady);
                packetWriter.Write(player[1].position);
                packetWriter.Write(player[1].grabPos);
                packetWriter.Write(player[1].viewAngle);
                packetWriter.Write(player[1].health);
                packetWriter.Write(player[2].health);
                packetWriter.Write(player[1].hasPowerUp);
                packetWriter.Write(player[2].hasPowerUp);
                packetWriter.Write(powerUp.alive);
                packetWriter.Write(gravityWell.alive);
                packetWriter.Write(gravityWell.deployed);
                packetWriter.Write(gravityWell.position);
                packetWriter.Write(ball.alive);
                packetWriter.Write(ball.position);
                packetWriter.Write(ball.charge);
                packetWriter.Write(ball.whoGrabbed);
                packetWriter.Write(ball.lastHitter);

                bool ballReleased = false;
                if (player[1].gamePadState.Triggers.Left == 0.0f &&
                    player[1].gamePadState.Triggers.Right == 0.0f)
                    ballReleased = true;

                packetWriter.Write(ballReleased);
            }
            if (player[2].playerType == LocalPlayer)
            {
                packetWriter.Write(player[2].alive);
                packetWriter.Write(player[2].playerReady);
                packetWriter.Write(player[2].position);
                packetWriter.Write(player[2].grabPos);
                packetWriter.Write(player[2].viewAngle);
                packetWriter.Write(ball.position);
                packetWriter.Write(ball.charge);
                packetWriter.Write(ball.whoGrabbed);
                packetWriter.Write(ball.lastHitter);

                bool ballReleased = false;
                if (ball.whoGrabbed == 2 &&
                    player[2].gamePadState.Triggers.Left == 0.0f &&
                    player[2].gamePadState.Triggers.Right == 0.0f)
                    ballReleased = true;

                packetWriter.Write(ballReleased);

                bool isGWGReleased = false;
                if (player[2].hasPowerUp &&
                    (player[2].gamePadState.Buttons.LeftShoulder == ButtonState.Pressed ||
                    player[2].gamePadState.Buttons.RightShoulder == ButtonState.Pressed))
                    isGWGReleased = true;

                packetWriter.Write(isGWGReleased);
            }*/

            // Send the data to everyone in the session.
            gamer.SendData(packetWriter, SendDataOptions.InOrder);
        }

        /// <summary>
        /// Helper for reading incoming network packets.
        /// </summary>
        protected void ReadIncomingPackets(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(packetReader, out sender);

                // Discard packets sent by local gamers: we already know their state!
                if (sender.IsLocal)
                    continue;

                //Player ships
                int playerShipCount = packetReader.ReadInt32();

                for (int i = 0; i < playerShipCount; i++)
                {
                    int shipID = packetReader.ReadInt32();
                    EntityEventType type = (EntityEventType)packetReader.ReadInt32();

                    switch (type)
                    {
                        case EntityEventType.State:
                            if (playerShips[shipID] == null) //Didn't exist, so this is a new entity
                            {
                                playerShips[shipID] = new Placeholder_PlayerShip(shipID);
                            }
                            playerShips[shipID].health = packetReader.ReadSingle();
                            playerShips[shipID].position = packetReader.ReadVector3();
                            playerShips[shipID].heading = packetReader.ReadQuaternion();
                            playerShips[shipID].velocity = packetReader.ReadVector3();
                            playerShips[shipID].rotation = packetReader.ReadVector3();
                            break;
                        case EntityEventType.Remove:
                            playerShips[shipID] = null;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Starts hosting a new network session.
        /// </summary>
        protected void CreateSession()
        {
            try
            {
                session = NetworkSession.Create(NetworkSessionType.SystemLink, maxLocalPlayers, maxNetworkPlayers);
                HookSessionEvents();
            }
            catch (Exception e)
            {
                networkErrorMessage = e.Message;
            }
        }


        /// <summary>
        /// Joins an existing network session.
        /// </summary>
        protected void JoinSession()
        {
            try
            {
                // Search for sessions.
                using (AvailableNetworkSessionCollection availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, maxLocalPlayers, null))
                {
                    if (availableSessions.Count == 0)
                    {
                        networkErrorMessage = "No network sessions found.";
                        return;
                    }

                    // Join the first session we found.
                    session = NetworkSession.Join(availableSessions[0]);

                    HookSessionEvents();
                }
            }
            catch (Exception e)
            {
                networkErrorMessage = e.Message;
            }
        }


        /// <summary>
        /// After creating or joining a network session, we must subscribe to
        /// some events so we will be notified when the session changes state.
        /// </summary>
        protected void HookSessionEvents()
        {
            session.GamerJoined += GamerJoinedEventHandler;
            session.SessionEnded += SessionEndedEventHandler;
        }


        /// <summary>
        /// This event handler will be called whenever a new gamer joins the session.
        /// </summary>
        protected void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            //Do something with the new player
        }

        protected void GamerLeftEventHandler(object sender, GamerLeftEventArgs e)
        {
            //Remove the player and the associated ship from game
        }

        /// <summary>
        /// Event handler notifies us when the network session has ended.
        /// </summary>
        protected void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            networkErrorMessage = e.EndReason.ToString();
            session.Dispose();
        }
    }
}
