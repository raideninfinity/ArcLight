using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class GameSession
    {
        public GamePlayer[] players = new GamePlayer[2];
        public int stage = 1;
        public int max_stage = 1;
        public bool movable = true;
        public Dictionary<string, bool> flags = new Dictionary<string, bool>();
        public bool game_over = false;
        public bool[] continue_prompt = { false, false };
        public int p1_last_type, p1_last_mode, p2_last_type, p2_last_mode;

        public GameSession(bool p1_active, int p1_type, int p1_mode, bool p2_active, int p2_type, int p2_mode)
        {
            if (p1_active)
            {
                players[0] = new GamePlayer(0, p1_type, p1_mode);
                p1_last_type = p1_type;
                p1_last_mode = p1_mode;
            }
            else players[0] = null;
            if (p2_active)
            {
                players[1] = new GamePlayer(1, p2_type, p2_mode);
                p2_last_type = p2_type;
                p2_last_mode = p2_mode;
            }
            else players[1] = null;
            if (p1_active && p2_active)
            {
                if (players[0].type == players[1].type) players[1].color = 1;
            }
        }

        public bool Player1Online()
        {
            return (players[0] != null);
        }

        public bool Player2Online()
        {
            return (players[1] != null);
        }

        public bool GameActive()
        {
            return Player1Online() || Player2Online();
        }

        public void ProgressStage()
        {
            stage += 1;
        }

        public GamePlayer player1 { get { return players[0]; } }
        public GamePlayer player2 { get { return players[1]; } }

        public void ResetBurstChain(GamePlayer player)
        {
            player.chain = 0;
            player.chain_time = 0;
        }

        public void AddBurstChain(GamePlayer player)
        {
            player.chain += 1;
            player.chain_time = (int)(Status.PlayerChainTime * 60);
            if (player.max_chain < player.chain)
            {
                player.max_chain = player.chain;
            }
        }

        public void UpdateBurstChain()
        {
            if (movable)
            {
                if (Player1Online())
                {
                    player1.chain_time -= 1;
                    if (player1.chain_time <= 0) player1.chain = 0;
                    Core.Session.GainEnergy(player1, Status.EnergyGainPerSecond / 60.0f);
                }
                if (Player2Online())
                {
                    player2.chain_time -= 1;
                    if (player2.chain_time <= 0) player2.chain = 0;
                    Core.Session.GainEnergy(player2, Status.EnergyGainPerSecond / 60.0f);
                }
            }
        }

        public void ConsumeLife(GamePlayer player)
        {
            player.lives -= 1;
            if (player.lives < 0)
            {
                players[player.index] = null;
                continue_prompt[player.index] = true;
            }
        }

        public void SetPlayer(int player, int type, int mode, bool intrude = false)
        {
            players[player] = new GamePlayer(player, type, mode);
            if (player == 0)
            {
                p1_last_type = type;
                p1_last_mode = mode;
            }
            else
            {
                p2_last_type = type;
                p2_last_mode = mode;
            }
            if (intrude) players[player].intruded = true;
            if (Player1Online() && Player2Online())
            {
                if (players[0].type == players[1].type) players[1].color = 1;
            }
        }

        public void GainEnergy(int player, float amount)
        {
            players[player].energy += amount;
            if (players[player].energy > 30000) { players[player].energy = 30000; }
        }

        public void GainEnergy(GamePlayer player, float amount)
        {
            GainEnergy(player.index, amount);
        }

        public void GainScore(int player, float amount, bool mult = true)
        {
            players[player].score += amount * (1 + (mult ? players[player].chain : 0));
        }

        public void GainScore(GamePlayer player, float amount, bool mult = true)
        {
            GainScore(player.index, amount, mult);
        }

        public void AllGainScore(float amount)
        {
            if (Player1Online()) GainScore(player1, amount, false);
            if (Player2Online()) GainScore(player2, amount, false);
        }

        public void ResetMaxChain()
        {
            if (Player1Online()) player1.max_chain = 0;
            if (Player2Online()) player2.max_chain = 0;
        }

        public bool Player1NoMiss()
        {
            return !player1.intruded && player1.miss == 0;
        }

        public bool Player2NoMiss()
        {
            return !player2.intruded && player2.miss == 0;
        }

        public void SetFlag(string key, bool value)
        {
            flags[key] = value;
        }

        public bool GetFlag(string key)
        {
            if (!flags.ContainsKey(key))
                return false;
            return flags[key];
        }

        public void InitStage()
        {
            ResetMaxChain();
            flags.Clear();
            if (Player1Online())
            {
                player1.intruded = false;
                player1.miss = 0;
            }
            if (Player2Online())
            {
                player2.intruded = false;
                player2.miss = 0;
            }
        }

    }
}
