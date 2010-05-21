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
    public class Network
    {
        private enum EntityEventType { State, Remove }
        private enum GamePacketType { PlayerShip, Ship, Asteroid }

        NetworkSession session;
        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        string networkErrorMessage = "";

        int maxLocalPlayers = 1;
        int maxNetworkPlayers = 8;

        /// <summary>
        /// Updates the state of the network session.
        /// </summary>
        protected void Updatesession()
        {
            // Make sure the session has not ended.
            if (session == null)
                return;

            // Send our local player's ship data to server
            foreach (LocalNetworkGamer gamer in session.LocalGamers)
            {
                SendPlayerShipData(gamer);
            }

            // Pump the underlying session object.
            session.Update();

            // Read any incoming packets.
            foreach (LocalNetworkGamer gamer in session.LocalGamers)
            {
                ReadIncomingPackets(gamer);
            }
        }

        public void HostGame()
        {
            if (Gamer.SignedInGamers.Count == 0 && Guide.IsVisible == false)
            {
                // If there are no profiles signed in, we cannot proceed.
                // Show the Guide so the user can sign in.
                Guide.ShowSignIn(maxLocalPlayers, false);
            }
            else
            {
                if (session == null)
                {
                    CreateSession();
                }
                else
                {
                    //FIXME: What to do when already in a session?
                }
            }
        }

        public void JoinGame()
        {
            
            if (Gamer.SignedInGamers.Count == 0 && Guide.IsVisible == false)
            {
                // If there are no profiles signed in, we cannot proceed.
                // Show the Guide so the user can sign in.
                Guide.ShowSignIn(maxLocalPlayers, false);
            }
            else
            {
                if (session == null)
                {
                    JoinSession();
                }
                else
                {
                    //FIXME: What to do when already in a session?
                }
            }
        }

        //Server sending player ship data to all other clients
        private void BroadCastPlayerShipData(LocalNetworkGamer server)
        {
            packetWriter.Write((Int32)GamePacketType.PlayerShip);

            int playerShipCount = EntityManager.get().playerShips.Count;
            packetWriter.Write(playerShipCount);

            for (int i = 0; i < playerShipCount; i++)
            {
                PlayerShip ship = (PlayerShip)EntityManager.get().playerShips[i];
                packetWriter.Write(ship.entityID);
                packetWriter.Write((Int32)EntityEventType.State);
                packetWriter.Write(ship.hull);
                packetWriter.Write(ship.position);
                packetWriter.Write(ship.heading);
                packetWriter.Write(ship.velocity);
                packetWriter.Write(ship.rotation);
            }

            // Send data of all player ships to all clients
            server.SendData(packetWriter, SendDataOptions.InOrder);
        }

        //Client sending local player's ship data to server
        private void SendPlayerShipData(LocalNetworkGamer gamer)
        {
            int playerShipCount = EntityManager.get().playerShips.Count;
            PlayerShip ship = null;

            for (int i = 0; i < playerShipCount; i++)
            {
                if (((PlayerShip)EntityManager.get().playerShips[i]).ownerID == gamer.Id)
                {
                    ship = (PlayerShip)EntityManager.get().playerShips[i];
                }
            }

            //If we didn't find the ship for the local player...
            if (ship == null)
            {
                return;
            }
            
            packetWriter.Write((Int32)GamePacketType.PlayerShip);
            packetWriter.Write(playerShipCount);
            packetWriter.Write(ship.entityID);
            packetWriter.Write((Int32)EntityEventType.State);
            packetWriter.Write(ship.hull);
            packetWriter.Write(ship.position);
            packetWriter.Write(ship.heading);
            packetWriter.Write(ship.velocity);
            packetWriter.Write(ship.rotation);

            // Send the data to server of the the session.
            gamer.SendData(packetWriter, SendDataOptions.InOrder, session.Host);
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

                GamePacketType type = (GamePacketType)packetReader.ReadInt32();
                switch (type)
                {
                    case GamePacketType.PlayerShip:
                        ReadPlayerShipData(gamer);
                        break;
                    default:
                        //Do nothing, unknown GamePacketType
                        break;
                }
            }
        }

        private void ReadPlayerShipData(LocalNetworkGamer gamer)
        {
            //Player ships
            int playerShipCount = packetReader.ReadInt32();

            for (int i = 0; i < playerShipCount; i++)
            {
                int shipID = packetReader.ReadInt32();
                PlayerShip ship = (PlayerShip)EntityManager.get().playerShips[shipID];

                //Ignore local gamers' shipdata, we don't need that
                if (ship.ownerID == gamer.Id)
                {
                    packetReader.ReadInt32();
                    packetReader.ReadSingle();
                    packetReader.ReadVector3();
                    packetReader.ReadQuaternion();
                    packetReader.ReadVector3();
                    packetReader.ReadVector3();
                }

                EntityEventType type = (EntityEventType)packetReader.ReadInt32();

                switch (type)
                {
                    case EntityEventType.State:
                        if (ship == null) //Didn't exist, so this is a new entity
                        {
                            //FIXME: Need a way to create a new player ship with entity manager
                            //ship = EntityManager.;
                        }
                        ship.hull = packetReader.ReadSingle();
                        ship.position = packetReader.ReadVector3();
                        ship.heading = packetReader.ReadQuaternion();
                        ship.velocity = packetReader.ReadVector3();
                        ship.rotation = packetReader.ReadVector3();
                        break;
                    case EntityEventType.Remove:
                        //FIXME: this is not the right way to destroy a player ship entity
                        ship = null;
                        break;
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
