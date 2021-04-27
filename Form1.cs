using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Omok
{
    public partial class Form1 : Form
    {
        // 인성아 좀 그만좀 컴파일해 ㅎㅎ
        int margin = 40; // 바둑판의 margin 크기
        int 눈size = 30; // 줄간격
        int 돌size = 28; //바둑돌의 크기
        int 화점size = 10; //바둑판의 큰 검은 점 크기

        Graphics g; //graphics 클래스 정의 = 그림 그리는 메소드들이 있음.
        Pen pen; //그림 그릴 때 필요 : 선 
        Brush wBrush, bBrush; // 그림 그릴 때 필요 : 백돌, 흑돌

        enum STONE { none, black, white };
        STONE[,] 바둑판 = new STONE[19, 19];
        bool flag = false; //false = 흑돌, true = 백돌
        bool imageFlag = false;
        int stoneCnt = 1; // 수순(몇 번 째 돌을 놓았는지)
        Font font = new Font("맑은 고딕", 10);  // 수순 출력용

        public Form1()
        {
            InitializeComponent();

            
            this.BackColor = Color.Orange; //Background-Color


            pen = new Pen(Color.Black); //선
            bBrush = new SolidBrush(Color.Black); //흑돌
            wBrush = new SolidBrush(Color.White); //백돌

            this.ClientSize = new Size(2 * margin + 18 * 눈size, 2 * margin + 18 * 눈size + menuStrip1.Height); //폼 크기를 지정
            //폼 크기를 코드로 지정하는 경우에는 ClientSize 사용


            

        }

        private void 끝내기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DrawBoard()
        {
            //Panell에 Graphics 객체 생성
            g = panel1.CreateGraphics();

            //세로선 19개 생성
            for (int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin + i * 눈size, margin),
                    new Point(margin + i * 눈size, margin + 18 * 눈size));
            }

            //가로선 19개 생성
            for (int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin, margin + i * 눈size),
                  new Point(margin + 18 * 눈size, margin + i * 눈size));
            }

            //화점 생성
            for(int x = 3; x <= 15; x += 6)
                for (int y = 3; y <= 15; y += 6)
                {
                    g.FillEllipse(bBrush,
                        margin + 눈size * x - 화점size / 2,
                        margin + 눈size * y - 화점size / 2,
                        화점size, 화점size);
                }

        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawBoard(); //바둑판 그리기
            DrawStone(); //화면을 줄였을 시 바둑돌이 사라짐으로 다시 한 번 돌을 그려 방지
        }

        // 오목인지 체크하는 메서드
        private void CheckOmok(int x, int y)
        {
            int cnt = 1;

            // 오른쪽 방향
            for (int i = x + 1; i <= 18; i++)
                if (바둑판[i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 왼쪽방향
            for (int i = x - 1; i >= 0; i--)
                if (바둑판[i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 아래 방향
            for (int i = y + 1; i <= 18; i++)
                if (바둑판[x, i] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 위 방향
            for (int i = y - 1; i >= 0; i--)
                if (바둑판[x, i] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 대각선 오른쪽 위방향
            for (int i = x + 1, j = y - 1; i <= 18 && j >= 0; i++, j--)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 대각선 왼쪽 아래 방향
            for (int i = x - 1, j = y + 1; i >= 0 && j <= 18; i--, j++)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 대각선 왼쪽 위방향
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 대각선 오른쪽 아래 방향
            for (int i = x + 1, j = y + 1; i <= 18 && j <= 18; i++, j++)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }
        }
        
        // 오목이 되었을 떄 처리하는 루틴 
        private void OmokComplete(int x, int y)
        {
            DialogResult res = MessageBox.Show(바둑판[x, y].ToString().ToUpper()
              + " Wins!\n새로운 게임을 시작할까요?", "게임 종료", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
                NewGame();
            else if (res == DialogResult.No)
                this.Close();
        }

        // 새로운 게임을 시작(초기화)
        private void NewGame()
        {
            imageFlag = true;
            flag = false;

            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    바둑판[x, y] = STONE.none;

            stoneCnt = 1;
            panel1.Refresh();
            DrawBoard();
            DrawStone();
        }
        
        // 자료구조에서 바둑돌의 값을 읽어서 다시 그려준다
        private void DrawStone()
        {
            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    if (바둑판[x, y] == STONE.white)
                        if (imageFlag == false)
                            g.FillEllipse(wBrush, margin + x * 눈size - 돌size / 2,
                              margin + y * 눈size - 돌size / 2, 돌size, 돌size);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../Images/whiteStone.png");
                            g.DrawImage(bmp, margin + x * 눈size - 돌size / 2,
                              margin + y * 눈size - 돌size / 2, 돌size, 돌size);
                        }

                    else if (바둑판[x, y] == STONE.black)
                        if (imageFlag == false)
                            g.FillEllipse(bBrush, margin + x * 눈size - 돌size / 2,
                              margin + y * 눈size - 돌size / 2, 돌size, 돌size);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../Images/blackStone.png");
                            g.DrawImage(bmp, margin + x * 눈size - 돌size / 2,
                              margin + y * 눈size - 돌size / 2, 돌size, 돌size);
                        }
        }

        // 오목돌을 놓기(흑과 백을 번갈아가면서)
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // e.X는 픽셀단위, x는 바둑판 좌표
            int x = (e.X - margin + 눈size / 2) / 눈size;
            int y = (e.Y - margin + 눈size / 2) / 눈size;

            if (바둑판[x, y] != STONE.none)
                return;

            // 바둑판[x,y] 에 돌을 그린다
            Rectangle r = new Rectangle(
              margin + 눈size * x - 돌size / 2,
              margin + 눈size * y - 돌size / 2,
              돌size, 돌size);

            // 검은돌 차례
            if (flag == false)
            {
                if (imageFlag == false)
                    g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/blackStone.png");//Bitmap을 만들어서 DrawImage() 메소드로 사각형 영역 안에 그려줌
                    g.DrawImage(bmp, r);
                }
                DrawStoneSequence(stoneCnt++, Brushes.White, r);
                flag = true;
                바둑판[x, y] = STONE.black;
            }
            else
            {
                if (imageFlag == false)
                    g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/whiteStone.png"); //Bitmap을 만들어서 DrawImage() 메소드로 사각형 영역 안에 그려줌
                    g.DrawImage(bmp, r);
                }
                DrawStoneSequence(stoneCnt++, Brushes.Black, r);
                flag = false;
                바둑판[x, y] = STONE.white;
            }
            CheckOmok(x, y);
        }

        // 수순을 돌의 중앙에 써줍니다
        private void DrawStoneSequence(int v, Brush color, Rectangle r)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(v.ToString(), font, color, r, stringFormat);
        }

        private void 그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = false;
        }

        private void 이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = true;
        }
        
    }
}
