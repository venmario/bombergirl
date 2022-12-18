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

namespace BomberManStudentChallange
{
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    public enum ImageBom
    {
        bom,
        bom2
    }

    public partial class FormGame : Form
    {
        Classes.Blok[,] objLevel = new Classes.Blok[11, 13];
        Classes.Bomber objBomber;
        List<PictureBox> listBlokHancur = new List<PictureBox>();
        List<PictureBox> listBlokTidakHancur = new List<PictureBox>();
        List<PictureBox> listBlokKosong = new List<PictureBox>();
        List<PictureBox> listBom = new List<PictureBox>();
        List<PictureBox> listLedakan = new List<PictureBox>();
        List<PictureBox> listMusuh = new List<PictureBox>();
        List<Timer> listTimer = new List<Timer>();
        int maxMusuh = 3;
        bool longFire = false;
        bool start = false;
        bool death = false;
        int bonusApi;
        int userLevel;
        Random generator = new Random();
        string resourcesPath = Application.StartupPath + "\\Music\\";
        WindowsMediaPlayer loopSound = new WindowsMediaPlayer();
        WindowsMediaPlayer normalSound = new WindowsMediaPlayer();
        int time = 120;

        public FormGame()
        {            
            InitializeComponent();
                       
        }
        
        private void FormGame_Load(object sender, EventArgs e)
        {
            loopSound.URL = resourcesPath + "BackgroundMusic.mp3";
            loopSound.settings.setMode("loop", true);
            userLevel = 1;
            Cover();
        }
        PictureBox coverGame;
        PictureBox startGame;
        PictureBox exitGame;
        private void Cover()
        {
            coverGame = new PictureBox();
            coverGame.Image = Properties.Resources.cover;
            coverGame.Size = new Size(13 * lebarBlok, 11 * tinggiBlok);
            coverGame.Location = new Point(0, 0);
            coverGame.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(coverGame);

            startGame = new PictureBox();
            startGame.Image = Properties.Resources.Start;
            startGame.Size = new Size(3 * lebarBlok, 2 * tinggiBlok);
            startGame.Location = new Point(280, 250);
            startGame.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(startGame);
            startGame.BringToFront();            

            exitGame = new PictureBox();
            exitGame.Image = Properties.Resources.Exit;
            exitGame.Size = new Size(3 * lebarBlok, 2 * tinggiBlok);
            exitGame.Location = new Point(280, 370);
            exitGame.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(exitGame);
            exitGame.BringToFront();

            startGame.Click += new System.EventHandler(StartGame_Click);
            exitGame.Click += new System.EventHandler(ExitGame_Click);
        }

        private void ExitGame_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            coverGame.Hide();
            startGame.Hide();
            exitGame.Hide();
            Level(1);
        }

        private Classes.Blok MencariArray(Control item)
        {
            Classes.Blok hasil = null;
            for (int i = 0; i < objLevel.GetLength(0); i++)
            {
                for (int j = 0; j < objLevel.GetLength(1); j++)
                {
                    if (item == objLevel[i, j].ObjekBlok)
                    {
                        hasil = objLevel[i, j];
                    }
                }
            }
            return hasil;
        }

        int tinggiBlok = 50;
        int lebarBlok = 50;
        string[] stringLineArray;
        Label labelTime;
        private void Level(int level)
        {
            start = true;
            listTimer.Add(timerMovementMusuh1);
            listTimer.Add(timerMovementMusuh2);
            listTimer.Add(timerMovementMusuh3);
            timerPintu.Enabled = true;
            timerGenerateMusuh.Enabled = true;
            timerGame.Enabled = true;
            string mapLevel = "";
            switch (level)
            {
                case 1:
                    mapLevel = Properties.Resources.Level1;
                    break;
                case 2:
                    mapLevel = Properties.Resources.Level2;
                    timerMovementMusuh1.Interval = 50;
                    timerMovementMusuh2.Interval = 50;
                    timerMovementMusuh3.Interval = 50;
                    break;
            }
            using (System.IO.StringReader stringReader = new System.IO.StringReader(mapLevel))
            {
                int posisiX = 0;
                int posisiY = 0;
                int inisialPosX = 0;
                int indexBaris = 0;
                int indexKolom = 0;
                string stringLine = "";
                while ((stringLine = stringReader.ReadLine()) != null)
                {
                    stringLineArray = stringLine.Split(' ');
                    Nullable<Classes.TipeBlok> tipeBlok = null;
                    foreach (string stringBlockChar in stringLineArray)
                    {
                        PictureBox pictBlok = new PictureBox();
                        pictBlok.Size = new Size(lebarBlok, tinggiBlok);
                        pictBlok.SizeMode = PictureBoxSizeMode.StretchImage;
                        switch (stringBlockChar)
                        {
                            case "D":
                                pictBlok.Image = Properties.Resources.tembok_yg_hancur;
                                pictBlok.BorderStyle = BorderStyle.FixedSingle;
                                listBlokHancur.Add(pictBlok);
                                tipeBlok = Classes.TipeBlok.Destructible;
                                //block yang bisa hancur
                                break;
                            case "V":
                                pictBlok.Image = Properties.Resources.pink;
                                tipeBlok = Classes.TipeBlok.Empty;
                                listBlokKosong.Add(pictBlok);
                                //tidak ada block
                                break;
                            case "N":
                                pictBlok.Image = Properties.Resources.tembok_keras;
                                pictBlok.BorderStyle = BorderStyle.FixedSingle;
                                listBlokTidakHancur.Add(pictBlok);
                                tipeBlok = Classes.TipeBlok.NonDestructible;
                                //block yg tidak bisa hancur
                                break;
                        }
                        pictBlok.Location = new Point(posisiX, posisiY);
                        this.Controls.Add(pictBlok);
                        pictBlok.BringToFront();
                        this.objLevel[indexBaris, indexKolom] = new Classes.Blok(pictBlok, tipeBlok.Value);
                        indexKolom++;
                        posisiX += lebarBlok;
                    }
                    indexBaris++;
                    indexKolom = 0;
                    posisiX = inisialPosX;
                    posisiY += tinggiBlok;
                }
                stringReader.Close();
            }
            bonusApi = generator.Next(0, listBlokHancur.Count);            

            PictureBox bomber = new PictureBox();
            bomber.Image = Properties.Resources.bomberman_kanan;
            bomber.BackgroundImage = Properties.Resources.pink;
            bomber.Size = new Size(lebarBlok, tinggiBlok);
            bomber.SizeMode = PictureBoxSizeMode.StretchImage;
            bomber.BackColor = Color.LightPink;
            this.posisiBomberBaris = 1;
            this.posisiBomberKolom = 1;
            bomber.Location = objLevel[posisiBomberBaris, posisiBomberKolom].ObjekBlok.Location;
            this.Controls.Add(bomber);
            bomber.BringToFront();
            objBomber = new Classes.Bomber(bomber);

            bonusFire = new PictureBox();
            bonusFire.Image = Properties.Resources.bonus_api;
            bonusFire.Size = new Size(lebarBlok, tinggiBlok);
            bonusFire.Location = listBlokHancur[bonusApi].Location;
            bonusFire.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(bonusFire);
            bonusFire.SendToBack();

            pintu = new PictureBox();
            pintu.Image = Properties.Resources.pintu;
            pintu.Size = new Size(lebarBlok, tinggiBlok);
            pintu.SizeMode = PictureBoxSizeMode.StretchImage;
            pintu.Location = listBlokKosong[generator.Next(0, listBlokKosong.Count)].Location;
            this.Controls.Add(pintu);
            pintu.SendToBack();

            labelTime = new Label();
            labelTime.Size = new Size(lebarBlok, tinggiBlok);
            labelTime.Image = Properties.Resources.tembok_keras;
            labelTime.Location = objLevel[0, 6].ObjekBlok.Location;
            labelTime.Font = new Font( "Microsoft Sans Serif", 14, FontStyle.Bold);
            labelTime.TextAlign = ContentAlignment.MiddleCenter;
            labelTime.Text = time.ToString();
            this.Controls.Add(labelTime);
            labelTime.BringToFront();
            timerTime.Enabled = true;
        }
        
        bool moveBomber = false;
        Direction bomberDirection = Direction.Down;
        Direction enemyDirection1 = Direction.Up;
        Direction enemyDirection2 = Direction.Up;
        Direction enemyDirection3 = Direction.Up;
        ImageBom efekBom = ImageBom.bom;
        int countDownBom = 3;
        int countDownLedakan = 1;
        PictureBox pictureBom;
        int posisiBomberBaris;
        int posisiBomberKolom;

        private void GetPlayerBlock(ref int baris, ref int kolom)
        {
            for (int i = 0; i < objLevel.GetLength(0); i++)
            {
                for (int j = 0; j < objLevel.GetLength(1); j++)
                {
                    if (objBomber.BomberSprite.Location == objLevel[i, j].ObjekBlok.Location)
                    {
                        baris = i;
                        kolom = j;
                    }
                }
            }
        }

        private void FormGame_KeyUp(object sender, KeyEventArgs e)
        {
            int b = 0;
            int k = 0;

            if (start )
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        objBomber.BomberSprite.Image = Properties.Resources.bomberman_kanan;
                        GetPlayerBlock(ref b, ref k);
                        if (k + 1 < objLevel.GetLength(1) && objLevel[b, k + 1].BlokTipe == Classes.TipeBlok.Empty)
                        {
                            moveBomber = true;
                            this.timerBomberMovement.Enabled = true;
                            bomberDirection = Direction.Right;
                            this.posisiBomberBaris = b;
                            this.posisiBomberKolom = k;
                        }
                        break;
                    case Keys.Left:
                        objBomber.BomberSprite.Image = Properties.Resources.bomberman_kiri;
                        GetPlayerBlock(ref b, ref k);
                        if (k - 1 >= 0 && objLevel[b, k - 1].BlokTipe == Classes.TipeBlok.Empty)
                        {
                            moveBomber = true;
                            this.timerBomberMovement.Enabled = true;
                            bomberDirection = Direction.Left;
                            this.posisiBomberBaris = b;
                            this.posisiBomberKolom = k;
                        }
                        break;
                    case Keys.Up:
                        objBomber.BomberSprite.Image = Properties.Resources.bomberman_belakang;
                        GetPlayerBlock(ref b, ref k);
                        if (b - 1 >= 0 && objLevel[b - 1, k].BlokTipe == Classes.TipeBlok.Empty)
                        {
                            moveBomber = true;
                            this.timerBomberMovement.Enabled = true;
                            bomberDirection = Direction.Up;
                            this.posisiBomberBaris = b;
                            this.posisiBomberKolom = k;
                        }
                        break;
                    case Keys.Down:
                        objBomber.BomberSprite.Image = Properties.Resources.bomberman_depan;
                        GetPlayerBlock(ref b, ref k);
                        if (b + 1 < objLevel.GetLength(0) && objLevel[b + 1, k].BlokTipe == Classes.TipeBlok.Empty)
                        {
                            moveBomber = true;
                            this.timerBomberMovement.Enabled = true;
                            bomberDirection = Direction.Down;
                            this.posisiBomberBaris = b;
                            this.posisiBomberKolom = k;
                        }
                        break;
                    case Keys.B:
                        if (listBom.Count < 1)
                        {
                            pictureBom = new PictureBox();
                            pictureBom.Image = Properties.Resources.bom;
                            pictureBom.BackgroundImage = Properties.Resources.pink;
                            pictureBom.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBom.Location = objLevel[posisiBomberBaris, posisiBomberKolom].ObjekBlok.Location;
                            pictureBom.BackColor = Color.LightPink;
                            pictureBom.Size = this.objBomber.BomberSprite.Size;
                            this.listBom.Add(pictureBom);
                            this.Controls.Add(pictureBom);
                            pictureBom.BringToFront();
                            objBomber.BomberSprite.BringToFront();
                            timerBom.Enabled = true;
                        }
                        break;
                }
            }
            
            
        }

        private void timerBomberMovement_Tick(object sender, EventArgs e)
        {
            if (moveBomber)
            {
                int unitInX = 10;
                int unitInY = 10;
                int b = 0;
                int k = 0;
                switch (bomberDirection)
                {
                    case Direction.Right:
                        objBomber.BomberSprite.Location = new Point(objBomber.BomberSprite.Location.X + unitInX, objBomber.BomberSprite.Location.Y);
                        GetPlayerBlock(ref b, ref k);
                        if (objLevel[posisiBomberBaris, posisiBomberKolom + 1].ObjekBlok.Location == objBomber.BomberSprite.Location)
                        {
                            moveBomber = false;
                            this.timerBomberMovement.Enabled = false;
                            posisiBomberBaris = b;
                            posisiBomberKolom = k;
                        }
                        break;
                    case Direction.Left:
                        objBomber.BomberSprite.Location = new Point(objBomber.BomberSprite.Location.X - unitInX, objBomber.BomberSprite.Location.Y);
                        GetPlayerBlock(ref b, ref k);
                        if (objLevel[posisiBomberBaris, posisiBomberKolom - 1].ObjekBlok.Location == objBomber.BomberSprite.Location)
                        {
                            moveBomber = false;
                            this.timerBomberMovement.Enabled = false;
                            posisiBomberBaris = b;
                            posisiBomberKolom = k;
                        }
                        break;
                    case Direction.Up:
                        objBomber.BomberSprite.Location = new Point(objBomber.BomberSprite.Location.X, objBomber.BomberSprite.Location.Y - unitInY);
                        GetPlayerBlock(ref b, ref k);
                        if (objLevel[posisiBomberBaris - 1, posisiBomberKolom].ObjekBlok.Location == objBomber.BomberSprite.Location)
                        {
                            moveBomber = false;
                            this.timerBomberMovement.Enabled = false;
                            posisiBomberBaris = b;
                            posisiBomberKolom = k;
                        }
                        break;
                    case Direction.Down:
                        objBomber.BomberSprite.Location = new Point(objBomber.BomberSprite.Location.X, objBomber.BomberSprite.Location.Y + unitInY);
                        GetPlayerBlock(ref b, ref k);
                        if (objLevel[posisiBomberBaris + 1, posisiBomberKolom].ObjekBlok.Location == objBomber.BomberSprite.Location)
                        {
                            moveBomber = false;
                            this.timerBomberMovement.Enabled = false;
                            posisiBomberBaris = b;
                            posisiBomberKolom = k;
                        }
                        break;
                }
            }
        }

        private void timerBom_Tick(object sender, EventArgs e)
        {
            countDownBom--;
            switch (efekBom)
            {
                case ImageBom.bom:
                    efekBom = ImageBom.bom2;
                    pictureBom.Image = Properties.Resources.bom2;
                    break;
                case ImageBom.bom2:
                    efekBom = ImageBom.bom;
                    pictureBom.Image = Properties.Resources.bom;
                    break;
            }
            if (countDownBom == 0)
            {
                BomMeledak();
                normalSound.URL = resourcesPath + "ledakan.mp3";
            }
        }
        PictureBox ledakanBomVer;
        PictureBox ledakanBomHor;
        private void BomMeledak()
        {
            timerBom.Enabled = false;

            ledakanBomVer = new PictureBox();            
            ledakanBomVer.Image = Properties.Resources.api_vertikal;
            ledakanBomVer.BackColor = Color.Transparent;
            ledakanBomVer.Width = 50;            
            ledakanBomVer.SizeMode = PictureBoxSizeMode.StretchImage;            
            ledakanBomVer.BackColor = Color.Transparent;  
            
            ledakanBomHor = new PictureBox();
            ledakanBomHor.Image = Properties.Resources.api_horizontal;
            ledakanBomHor.BackColor = Color.Transparent;            
            ledakanBomHor.Height = 50;
            ledakanBomHor.SizeMode = PictureBoxSizeMode.StretchImage;
            ledakanBomHor.BackColor = Color.Transparent;
            
            

            if (longFire)
            {
                ledakanBomVer.Height = 250;
                ledakanBomVer.Location = new Point(pictureBom.Location.X, pictureBom.Location.Y - 100);
                ledakanBomHor.Width = 250;
                ledakanBomHor.Location = new Point(pictureBom.Location.X - 100, pictureBom.Location.Y);
            }
            else
            {
                ledakanBomVer.Height = 150;
                ledakanBomVer.Location = new Point(pictureBom.Location.X, pictureBom.Location.Y - 50);
                ledakanBomHor.Width = 150;
                ledakanBomHor.Location = new Point(pictureBom.Location.X - 50, pictureBom.Location.Y);
            }

            listLedakan.Add(ledakanBomVer);
            this.Controls.Add(ledakanBomVer);
            ledakanBomVer.BringToFront();
            listLedakan.Add(ledakanBomHor);
            this.Controls.Add(ledakanBomHor);
            listLedakan.Add(ledakanBomHor);
            ledakanBomHor.BringToFront();

            countDownBom = 3;
            timerLedakan.Enabled = true;
            listBom[0].Dispose();
            listBom.RemoveAt(0);
            Intersect();
        }

        private void timerLedakan_Tick(object sender, EventArgs e)
        {
            Intersect();
            countDownLedakan--;

            if (countDownLedakan == 0 && death == false)
            {
                LedakanBom();                
            }
            else if (countDownLedakan == 0 && death)
            {
                LedakanBom();
            }
        }
        

        private void Intersect()
        {
            for (int i = 0; i < listMusuh.Count; i++)
            {
                if (ledakanBomHor.Bounds.IntersectsWith(listMusuh[i].Bounds))
                {
                    listTimer[i].Enabled = false;
                    listMusuh[i].Hide();
                    listMusuh[i].Image = null;
                }              
            }

            for (int i = 0; i < listMusuh.Count; i++)
            {
                if (ledakanBomVer.Bounds.IntersectsWith(listMusuh[i].Bounds))
                {
                    listTimer[i].Enabled = false;
                    listMusuh[i].Hide();
                    listMusuh[i].Image = null;
                }
            }

            for (int i = 0; i < listLedakan.Count; i++)
            {
                if (objBomber.BomberSprite.Bounds.IntersectsWith(listLedakan[i].Bounds))
                {
                    GameOver();
                    death = true;
                }
            }
        }

        private void LedakanBom()
        {
            listLedakan[0].Dispose();
            listLedakan[1].Dispose();
            listLedakan.Clear();
            timerLedakan.Enabled = false;
            countDownLedakan = 2;
            BlokHancur();

        }
        private void Reset()
        {
            timerMovementMusuh1.Enabled = false;
            timerMovementMusuh2.Enabled = false;
            timerMovementMusuh3.Enabled = false;
            timerBom.Enabled = false;
            timerLedakan.Enabled = false;
            timerBomberMovement.Enabled = false;
            timerGame.Enabled = false;
            timerPintu.Enabled = false;
            timerCekBonus.Enabled = false;
            timerGenerateMusuh.Enabled = false;
            timerTime.Enabled = false;
            time = 120;
            listBlokHancur.Clear();
            listBlokKosong.Clear();
            listBlokTidakHancur.Clear();
            listLedakan.Clear();
            listMusuh.Clear();
            listBom.Clear();
            listTimer.Clear();
            
        }
        private void GameOver()
        {
            Reset();
            longFire = false;
            death = false;
            PictureBox gameOver = new PictureBox();
            gameOver.Image = Properties.Resources.game_over;
            gameOver.Size = new Size(13 * lebarBlok, 11 * tinggiBlok);
            gameOver.SizeMode = PictureBoxSizeMode.StretchImage;
            gameOver.Location = new Point(0,0);
            this.Controls.Add(gameOver);
            gameOver.BringToFront();

            normalSound.URL = resourcesPath + "Failed.mp3";

            newGame = new PictureBox();
            newGame.Image = Properties.Resources.newgame;
            newGame.Size = new Size(3 * lebarBlok, 2 * tinggiBlok);
            newGame.Location = new Point(280, 250);
            newGame.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(newGame);
            newGame.BringToFront();

            exitGame.Show();
            exitGame.BringToFront();

            newGame.Click += new System.EventHandler(newGame_Click);
        }
        PictureBox newGame;
        private void newGame_Click(object sender, EventArgs e)
        {
            newGame.Hide();
            NewGame();            
        }        
        
        PictureBox bonusFire;
        private void BlokHancur()
        {            
            for (int i = 0; i < listBlokHancur.Count; i++)
            {
                if (ledakanBomHor.Bounds.IntersectsWith(listBlokHancur[i].Bounds))
                {
                    if (i == bonusApi)
                    {
                        bonusFire.BringToFront();
                        timerCekBonus.Enabled = true;
                    }
                    listBlokHancur[i].Image = Properties.Resources.pink;
                    listBlokHancur[i].BorderStyle = BorderStyle.None;
                    MencariArray(listBlokHancur[i]).BlokTipe = Classes.TipeBlok.Empty;
                }
                else if (ledakanBomVer.Bounds.IntersectsWith(listBlokHancur[i].Bounds))
                {
                    if (i == bonusApi)
                    {
                        bonusFire.BringToFront();
                        timerCekBonus.Enabled = true;
                    }
                    listBlokHancur[i].Image = Properties.Resources.pink;
                    listBlokHancur[i].BorderStyle = BorderStyle.None;
                    MencariArray(listBlokHancur[i]).BlokTipe = Classes.TipeBlok.Empty;
                }
            }
        }

        private void timerGenerateMusuh_Tick(object sender, EventArgs e)
        {
            PictureBox pictMusuh = new PictureBox();
            pictMusuh.Image = Properties.Resources.monster;
            pictMusuh.BackgroundImage = Properties.Resources.pink;
            pictMusuh.Size = new Size(lebarBlok, tinggiBlok);
            pictMusuh.BackColor = Color.Transparent;
            pictMusuh.SizeMode = PictureBoxSizeMode.StretchImage;
            pictMusuh.Location = listBlokKosong[generator.Next(14, listBlokKosong.Count)].Location;
            listMusuh.Add(pictMusuh);
            this.Controls.Add(pictMusuh);
            pictMusuh.BringToFront();

            if (listMusuh.Count == maxMusuh)
            {                
                timerGenerateMusuh.Enabled = false;
                MoveMusuh(0,timerMovementMusuh1,ref moveEnemy1,ref posisiMusuhBaris1,ref posisiMusuhKolom1,ref enemyDirection1);
                MoveMusuh(1, timerMovementMusuh2, ref moveEnemy2, ref posisiMusuhBaris2, ref posisiMusuhKolom2, ref enemyDirection2);
                MoveMusuh(2, timerMovementMusuh3, ref moveEnemy3, ref posisiMusuhBaris3, ref posisiMusuhKolom3, ref enemyDirection3);                
            }            
        }

        private void GetEnemyBlock(Control item, ref int baris, ref int kolom)
        {
            for (int i = 0; i < objLevel.GetLength(0); i++)
            {
                for (int j = 0; j < objLevel.GetLength(1); j++)
                {
                    if (item.Location == objLevel[i, j].ObjekBlok.Location)
                    {
                        baris = i;
                        kolom = j;
                    }
                }
            }
        }

        int posisiMusuhBaris1;
        int posisiMusuhKolom1;
        int posisiMusuhBaris2;
        int posisiMusuhKolom2;
        int posisiMusuhBaris3;
        int posisiMusuhKolom3;
        bool moveEnemy1 = false;
        bool moveEnemy2 = false;
        bool moveEnemy3 = false;

        private void MoveMusuh(int musuh, Timer timerMoveMusuh,ref bool move,ref int barisMusuh,ref int kolomMusuh, ref Direction enemyDirect)
        {
            
            int arah = generator.Next(1, 5);
            int b = 0;
            int k = 0;
            barisMusuh = 0;
            kolomMusuh = 0;
            switch (arah)
            {
                case 1:
                    GetEnemyBlock(listMusuh[musuh], ref b, ref k);
                    if (b - 1 >= 0 && objLevel[b - 1, k].BlokTipe == Classes.TipeBlok.Empty)
                    {
                        move = true;
                        timerMoveMusuh.Enabled = true;
                        enemyDirect = Direction.Up;
                        barisMusuh = b;
                        kolomMusuh = k;                        
                    }
                    else
                    {
                        MoveMusuh(musuh,timerMoveMusuh,ref move,ref barisMusuh,ref kolomMusuh,ref enemyDirect);
                    }
                    break;
                case 2:
                    GetEnemyBlock(listMusuh[musuh], ref b, ref k);
                    if (b + 1 < objLevel.GetLength(0) && objLevel[b + 1, k].BlokTipe == Classes.TipeBlok.Empty)
                    {
                        move = true;
                        timerMoveMusuh.Enabled = true;
                        enemyDirect = Direction.Down;
                        barisMusuh = b;
                        kolomMusuh = k;
                    }
                    else
                    {
                        MoveMusuh(musuh, timerMoveMusuh, ref move, ref barisMusuh, ref kolomMusuh, ref enemyDirect);
                    }
                    break;
                case 3:
                    GetEnemyBlock(listMusuh[musuh], ref b, ref k);
                    if (k - 1 >= 0 && objLevel[b, k - 1].BlokTipe == Classes.TipeBlok.Empty)
                    {
                        move = true;
                        timerMoveMusuh.Enabled = true;
                        enemyDirect = Direction.Left;
                        barisMusuh = b;
                        kolomMusuh = k;
                    }
                    else
                    {
                        MoveMusuh(musuh, timerMoveMusuh, ref move, ref barisMusuh, ref kolomMusuh, ref enemyDirect);
                    }
                    break;
                case 4:
                    GetEnemyBlock(listMusuh[musuh], ref b, ref k);
                    if (k + 1 < objLevel.GetLength(1) && objLevel[b, k + 1].BlokTipe == Classes.TipeBlok.Empty)
                    {
                        move = true;
                        timerMoveMusuh.Enabled = true;
                        enemyDirect = Direction.Right;
                        barisMusuh = b;
                        kolomMusuh = k;
                    }
                    else
                    {
                        MoveMusuh(musuh, timerMoveMusuh, ref move, ref barisMusuh, ref kolomMusuh, ref enemyDirect);
                    }
                    break;
            }
        }

        private void timerMovementMusuh1_Tick(object sender, EventArgs e)
        {           
            int b = 0, k = 0;
            switch (enemyDirection1)
            {
                case Direction.Right:
                    listMusuh[0].Location = new Point(listMusuh[0].Location.X + 10, listMusuh[0].Location.Y);
                    GetEnemyBlock(listMusuh[0], ref b, ref k);
                    if (objLevel[posisiMusuhBaris1, posisiMusuhKolom1 + 1].ObjekBlok.Location == listMusuh[0].Location)
                    {
                        moveEnemy1 = false;
                        timerMovementMusuh1.Enabled = false;                        
                        posisiMusuhBaris1 = b;
                        posisiMusuhKolom1 = k;
                        MoveMusuh(0,timerMovementMusuh1,ref moveEnemy1,ref posisiMusuhBaris1,ref posisiMusuhKolom1,ref enemyDirection1);
                    }
                    break;
                case Direction.Left:
                    listMusuh[0].Location = new Point(listMusuh[0].Location.X - 10, listMusuh[0].Location.Y);
                    GetEnemyBlock(listMusuh[0], ref b, ref k);
                    if (objLevel[posisiMusuhBaris1, posisiMusuhKolom1 - 1].ObjekBlok.Location == listMusuh[0].Location)
                    {
                        moveEnemy1 = false;
                        timerMovementMusuh1.Enabled = false;
                        posisiMusuhBaris1 = b;
                        posisiMusuhKolom1 = k;
                        MoveMusuh(0, timerMovementMusuh1, ref moveEnemy1, ref posisiMusuhBaris1, ref posisiMusuhKolom1, ref enemyDirection1);
                    }
                    break;
                case Direction.Up:
                    listMusuh[0].Location = new Point(listMusuh[0].Location.X, listMusuh[0].Location.Y-10);
                    GetEnemyBlock(listMusuh[0], ref b, ref k);
                    if (objLevel[posisiMusuhBaris1 - 1, posisiMusuhKolom1].ObjekBlok.Location == listMusuh[0].Location)
                    {
                        moveEnemy1 = false;
                        timerMovementMusuh1.Enabled = false;
                        posisiMusuhBaris1 = b;
                        posisiMusuhKolom1 = k;
                        MoveMusuh(0, timerMovementMusuh1, ref moveEnemy1, ref posisiMusuhBaris1, ref posisiMusuhKolom1, ref enemyDirection1);
                    }
                        break;
                case Direction.Down:
                    listMusuh[0].Location = new Point(listMusuh[0].Location.X, listMusuh[0].Location.Y + 10);
                    GetEnemyBlock(listMusuh[0], ref b, ref k);
                    if (objLevel[posisiMusuhBaris1 + 1, posisiMusuhKolom1].ObjekBlok.Location == listMusuh[0].Location)
                    {
                        moveEnemy1 = false;
                        timerMovementMusuh1.Enabled = false;
                        posisiMusuhBaris1 = b;
                        posisiMusuhKolom1 = k;
                        MoveMusuh(0, timerMovementMusuh1, ref moveEnemy1, ref posisiMusuhBaris1, ref posisiMusuhKolom1, ref enemyDirection1);
                    }
                    break;
            }
        }

        private void timerMovementMusuh2_Tick(object sender, EventArgs e)
        {
            int b = 0, k = 0;
            switch (enemyDirection2)
            {
                case Direction.Right:
                    listMusuh[1].Location = new Point(listMusuh[1].Location.X + 10, listMusuh[1].Location.Y);
                    GetEnemyBlock(listMusuh[1], ref b, ref k);
                    if (objLevel[posisiMusuhBaris2, posisiMusuhKolom2 + 1].ObjekBlok.Location == listMusuh[1].Location)
                    {
                        moveEnemy2 = false;
                        timerMovementMusuh2.Enabled = false;
                        posisiMusuhBaris2 = b;
                        posisiMusuhKolom2 = k;
                        MoveMusuh(1, timerMovementMusuh2,ref moveEnemy2,ref posisiMusuhBaris2,ref posisiMusuhKolom2,ref enemyDirection2);
                    }
                    break;
                case Direction.Left:
                    listMusuh[1].Location = new Point(listMusuh[1].Location.X - 10, listMusuh[1].Location.Y);
                    GetEnemyBlock(listMusuh[1], ref b, ref k);
                    if (objLevel[posisiMusuhBaris2, posisiMusuhKolom2 - 1].ObjekBlok.Location == listMusuh[1].Location)
                    {
                        moveEnemy2 = false;
                        timerMovementMusuh2.Enabled = false;
                        posisiMusuhBaris2 = b;
                        posisiMusuhKolom2 = k;
                        MoveMusuh(1, timerMovementMusuh2, ref moveEnemy2, ref posisiMusuhBaris2, ref posisiMusuhKolom2, ref enemyDirection2);
                    }
                    break;
                case Direction.Up:
                    listMusuh[1].Location = new Point(listMusuh[1].Location.X, listMusuh[1].Location.Y - 10);
                    GetEnemyBlock(listMusuh[1], ref b, ref k);
                    if (objLevel[posisiMusuhBaris2 - 1, posisiMusuhKolom2].ObjekBlok.Location == listMusuh[1].Location)
                    {
                        moveEnemy2 = false;
                        timerMovementMusuh2.Enabled = false;
                        posisiMusuhBaris2 = b;
                        posisiMusuhKolom2 = k;
                        MoveMusuh(1, timerMovementMusuh2, ref moveEnemy2, ref posisiMusuhBaris2, ref posisiMusuhKolom2, ref enemyDirection2);
                    }
                    break;
                case Direction.Down:
                    listMusuh[1].Location = new Point(listMusuh[1].Location.X, listMusuh[1].Location.Y + 10);
                    GetEnemyBlock(listMusuh[1], ref b, ref k);
                    if (objLevel[posisiMusuhBaris2 + 1, posisiMusuhKolom2].ObjekBlok.Location == listMusuh[1].Location)
                    {
                        moveEnemy2 = false;
                        timerMovementMusuh2.Enabled = false;
                        posisiMusuhBaris2 = b;
                        posisiMusuhKolom2 = k;
                        MoveMusuh(1, timerMovementMusuh2, ref moveEnemy2, ref posisiMusuhBaris2, ref posisiMusuhKolom2, ref enemyDirection2);
                    }
                    break;
            }
        }

        private void timerMovementMusuh3_Tick(object sender, EventArgs e)
        {
            int b = 0, k = 0;
            switch (enemyDirection3)
            {
                case Direction.Right:
                    listMusuh[2].Location = new Point(listMusuh[2].Location.X + 10, listMusuh[2].Location.Y);
                    GetEnemyBlock(listMusuh[2], ref b, ref k);
                    if (objLevel[posisiMusuhBaris3, posisiMusuhKolom3 + 1].ObjekBlok.Location == listMusuh[2].Location)
                    {
                        moveEnemy3 = false;
                        timerMovementMusuh3.Enabled = false;
                        posisiMusuhBaris3 = b;
                        posisiMusuhKolom3 = k;
                        MoveMusuh(2, timerMovementMusuh3, ref moveEnemy3, ref posisiMusuhBaris3, ref posisiMusuhKolom3, ref enemyDirection3);
                    }
                    break;
                case Direction.Left:
                    listMusuh[2].Location = new Point(listMusuh[2].Location.X - 10, listMusuh[2].Location.Y);
                    GetEnemyBlock(listMusuh[2], ref b, ref k);
                    if (objLevel[posisiMusuhBaris3, posisiMusuhKolom3 - 1].ObjekBlok.Location == listMusuh[2].Location)
                    {
                        moveEnemy3 = false;
                        timerMovementMusuh3.Enabled = false;
                        posisiMusuhBaris3 = b;
                        posisiMusuhKolom3 = k;
                        MoveMusuh(2, timerMovementMusuh3, ref moveEnemy3, ref posisiMusuhBaris3, ref posisiMusuhKolom3, ref enemyDirection3);
                    }
                    break;
                case Direction.Up:
                    listMusuh[2].Location = new Point(listMusuh[2].Location.X, listMusuh[2].Location.Y - 10);
                    GetEnemyBlock(listMusuh[2], ref b, ref k);
                    if (objLevel[posisiMusuhBaris3 - 1, posisiMusuhKolom3].ObjekBlok.Location == listMusuh[2].Location)
                    {
                        moveEnemy3 = false;
                        timerMovementMusuh3.Enabled = false;
                        posisiMusuhBaris3 = b;
                        posisiMusuhKolom3 = k;
                        MoveMusuh(2, timerMovementMusuh3, ref moveEnemy3, ref posisiMusuhBaris3, ref posisiMusuhKolom3, ref enemyDirection3);
                    }
                    break;
                case Direction.Down:
                    listMusuh[2].Location = new Point(listMusuh[2].Location.X, listMusuh[2].Location.Y + 10);
                    GetEnemyBlock(listMusuh[2], ref b, ref k);
                    if (objLevel[posisiMusuhBaris3 + 1, posisiMusuhKolom3].ObjekBlok.Location == listMusuh[2].Location)
                    {
                        moveEnemy3 = false;
                        timerMovementMusuh3.Enabled = false;
                        posisiMusuhBaris3 = b;
                        posisiMusuhKolom3 = k;
                        MoveMusuh(2, timerMovementMusuh3, ref moveEnemy3, ref posisiMusuhBaris3, ref posisiMusuhKolom3, ref enemyDirection3);
                    }
                    break;
            }
        }

        private void timerGame_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listMusuh.Count; i++)
            {
                if (listMusuh[i].Bounds.IntersectsWith(objBomber.BomberSprite.Bounds))
                {
                    if (listMusuh[i].Image != null)
                    {
                        GameOver();
                    }
                }
            }
        }
        PictureBox pintu;
        bool nextLevel = false;
        private void timerPintu_Tick(object sender, EventArgs e)
        {
            if (listMusuh[0].Image == null && listMusuh[1].Image == null && listMusuh[2].Image == null)
            {
                listMusuh.Clear();
                pintu.BringToFront();
                nextLevel = true;
                timerCekBonus.Enabled = true;
                timerPintu.Enabled = false;
            }
        }

        private void timerCekBonus_Tick(object sender, EventArgs e)
        {
            if (objBomber.BomberSprite.Bounds.IntersectsWith(bonusFire.Bounds))
            {
                longFire = true;
                bonusFire.Image = Properties.Resources.pink;
                bonusFire.SendToBack();
                for (int i = 0; i < listMusuh.Count; i++)
                {
                    listMusuh[i].BringToFront();
                }
            }

            else if (nextLevel)
            {
                if (userLevel == 1)
                {
                    if (objBomber.BomberSprite.Bounds.IntersectsWith(pintu.Bounds))
                    {
                        nextLevel = false;
                        userLevel++;
                        Reset();
                        Level(userLevel);
                    }
                }
                else
                {
                    if (objBomber.BomberSprite.Bounds.IntersectsWith(pintu.Bounds))
                    {
                        PictureBox tamat = new PictureBox();
                        tamat.Image = Properties.Resources.congrats;
                        tamat.Size = new Size(13 * lebarBlok, 11 * tinggiBlok);
                        tamat.SizeMode = PictureBoxSizeMode.StretchImage;
                        tamat.Location = new Point(0, 0);
                        this.Controls.Add(tamat);
                        tamat.BringToFront();
                        exitGame.Show();
                        exitGame.BringToFront();
                        normalSound.URL = resourcesPath + "Happy.mp3";
                    }
                }
                
            }          
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {            
            time--;
            labelTime.Text = time.ToString();
            if (time == 0)
            {
                GameOver();
                MessageBox.Show("Your time has end");
            }
        }
    }
}