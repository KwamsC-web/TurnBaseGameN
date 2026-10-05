using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace TurnBaseGameN
{
    public partial class Form1 : Form
    {
        private Player player1 = new Player("XIAO", 100, 10, 3, 60);
        private EnemyPlayer player2 = new EnemyPlayer("RONIN", 100, 10, 3, 60);
        private Player currentPlayer;
        private Player opponent;

        private WindowsMediaPlayer mediaPlayer;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            mediaPlayer = new WindowsMediaPlayer();

        }


        private async Task ShowAttackEffect(PictureBox targetPictureBox)
        {
            mediaPlayer.URL = "";
            mediaPlayer.controls.play();

            Image originalImage = targetPictureBox.Image;
            targetPictureBox.Image = Properties.Resources.claws_claw_scratch;


            await Task.Delay(500);
            targetPictureBox.Image = originalImage;
            mediaPlayer.controls.play();
        }

        private async Task ShowSkillEffect(PictureBox targetPictureBox)
        {
            mediaPlayer.URL = "";
            mediaPlayer.controls.play();

            Image originalImage = targetPictureBox.Image;
            targetPictureBox.Image = Properties.Resources.Effects1;


            await Task.Delay(500);
            targetPictureBox.Image = originalImage;
            mediaPlayer.controls.play();
        }

        private void InitializeGame()
        {
            currentPlayer = player1;
            opponent = player2;
            progressBar1.Maximum = progressBar2.Maximum = 100;
            progressBar3.Maximum = progressBar4.Maximum = 60;
            UpdateUI();
        }

        private void UpdateUI()
        {
            progressBar1.Value = player1.Health;
            progressBar2.Value = player2.Health;

            progressBar3.Value = Math.Min(player1.Mana, progressBar3.Maximum);
            progressBar4.Value = Math.Min(player2.Mana, progressBar4.Maximum);

            if (opponent.Mana == 60)
            {
                progressBar3.Value = progressBar4.Value = 60;
            }
        }

        private async void SwapTurns()
        {
            Player temp = currentPlayer;
            currentPlayer = opponent;
            opponent = temp;
            label1.Text = $"{currentPlayer.Name}'s Turn";

            if (currentPlayer is EnemyPlayer aIPlayer)
            {
                if (aIPlayer.TakeTurn(opponent))
                {
                    await Task.Delay(500);
                    await ShowAttackEffect(pictureBox2);
                }
                else
                {
                    await Task.Delay(1000);
                    await ShowSkillEffect(pictureBox2);
                }
                UpdateUI();
                if (opponent.Health <= 0)
                {
                    MessageBox.Show($"{currentPlayer.Name}'s win", "Game Over");
                    InitializeGame();
                    return;
                }
                SwapTurns();
            }
        }

        private void AskPermissionBeforeQuite(object sender, FormClosingEventArgs e)
        {
            DialogResult YesOrNO = MessageBox.Show("Are You Sure To Quit ?", "Turn-Base-Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sender as Button != button3 && YesOrNO == DialogResult.No) e.Cancel = true;
            if (sender as Button == button3 && YesOrNO == DialogResult.Yes) Environment.Exit(0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            AskPermissionBeforeQuite(sender, e as FormClosingEventArgs);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            switch (currentPlayer.UseSkill(opponent))
            {
                case 1:
                    await ShowSkillEffect(pictureBox3);
                    UpdateUI();
                    if (opponent.Health <= 0)
                    {
                        MessageBox.Show($"{currentPlayer.Name}'s win", "Game Over");
                        InitializeGame();
                        return;
                    }
                    break;
                case 2:
                    MessageBox.Show($"{currentPlayer.Name} missed the skill", "Miss");
                    break;
                default:
                    MessageBox.Show("Not enough mana to use skill!", "Skill Fail");
                    break;
            }
            SwapTurns();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await ShowAttackEffect(pictureBox3);
            if (currentPlayer.AttackPlayer(opponent))
            {
                UpdateUI();
                if (opponent.Health <= 0)
                {
                    MessageBox.Show($"{currentPlayer.Name}'s win", "Game Over");
                    InitializeGame();
                    return;
                }
            }
            else
            {
                MessageBox.Show($"{currentPlayer.Name} missed the attack", "Miss");
            }
            SwapTurns();
        }
    }
}
