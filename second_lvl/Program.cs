using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace second_lvl
{ }
// 1)   Добавить свои объекты в иерархию объектов, чтобы получился красивый задний фон, похожий на полет в звездном пространстве.
// 2)* Заменить кружочки картинками, используя метод DrawImage.

//Лукашенко Валентина
    class Program
    {
        
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);

        }

        static class Game
        {
            private  static List<Brushes> clr = new List<Brushes>();
            
            private static BufferedGraphicsContext _context;
            public static BufferedGraphics Buffer;
            // Свойства
            // Ширина и высота игрового поля
            public static int Width { get; set; }
            public static int Height { get; set; }
            static Game()
            {
            }


            public static BaseObject[] _objs;
            public static BackGroundImage plnt;
          


            public static void Load()
            {
                _objs = new BaseObject[30];
                for (int i = 0; i < _objs.Length; i++)
                {
                if (i%2==0) _objs[i] = new BaseObject(new Point(600, i * 20), new Point(-i, -i), new Size(30, 30));
                else _objs[i] = new Star(new Point(600, i * 20), new Point(-i, 0), new Size(5, 5));
                }
                plnt = new BackGroundImage(Width, Height);
              
            }


            public static void Draw()
            {
              

                Buffer.Graphics.Clear(Color.Black);
                foreach (BaseObject obj in _objs)
                    obj.Draw();
                plnt.Draw();
         
               
                Buffer.Render();
            }

            public static void Update()
            {
                foreach (BaseObject obj in _objs)
                    obj.Update();
            }


            public static void Init(Form form)
            {
                // Графическое устройство для вывода графики            
                Graphics g;
                // Предоставляет доступ к главному буферу графического контекста для текущего приложения
                _context = BufferedGraphicsManager.Current;
                g = form.CreateGraphics();
                // Создаем объект (поверхность рисования) и связываем его с формой
                // Запоминаем размеры формы
                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
                // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));


                Timer timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;


                Load();
            }

            private static void Timer_Tick(object sender, EventArgs e)
            {
                Draw();
                Update();
            }
        


        }

        class BaseObject
        {
            protected Point Pos;
            protected Point Dir;
            protected Size Size;
            private Image imgAsteroid;
            public BaseObject(Point pos, Point dir, Size size)
            {
                Pos = pos;
                Dir = dir;
                Size = size;
                imgAsteroid = Image.FromFile(@"asteroid.png");
            }


            public virtual void  Draw()
            {
                Game.Buffer.Graphics.DrawImage(imgAsteroid,Pos.X,Pos.Y, Size.Width, Size.Height);
            }
            public virtual void Update()
            {
                Pos.X = Pos.X + Dir.X;
                Pos.Y = Pos.Y + Dir.Y;
                if (Pos.X < 0) Dir.X = -Dir.X;
                if (Pos.X > Game.Width) Dir.X = -Dir.X;
                if (Pos.Y < 0) Dir.Y = -Dir.Y;
                if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
            }
        }


        class Star : BaseObject
        {
            public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
            {
            }

            public override void Draw()
            {
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
            }


            public override void Update()
            {
                Pos.X = Pos.X - Dir.X;
                if (Pos.X> Game.Width) Pos.X = 0;
            }

        }
        class Planete: BaseObject
        {
            private Brush color;
            public Planete(Point pos, Point dir, Size size, Brush color) : base(pos, dir, size)
            {
                this.color = color;
            }

            public override void Draw()
            {
                Game.Buffer.Graphics.FillEllipse(color, Pos.X, Pos.Y, Size.Width, Size.Height);
            }


            public override void Update()
            {
                base.Update();
            }


        }
        class BackGroundImage
        {
            public static Planete[] _planets;
            private List<Brush> clr;

            public BackGroundImage(int width, int height)
            {
                clr = new List<Brush>();
                clr.Add(Brushes.Beige);
                clr.Add(Brushes.Gray);
                clr.Add(Brushes.Aquamarine);
                clr.Add(Brushes.Red);
                clr.Add(Brushes.DarkOrange);
                clr.Add(Brushes.Orchid);
                clr.Add(Brushes.DarkGoldenrod);
                clr.Add(Brushes.Orange);
                clr.Add(Brushes.LawnGreen);

                Random rnd = new Random();

                _planets = new Planete[8];
                for (int i = 0; i < 8; i++)
                {
                    int k = rnd.Next(3, 10);
                    _planets[i] = new Planete(new Point((int)(width - 70*i-50), (int)(Math.Exp(i))), new Point(0, 0), new Size(7*k, 7 * k), clr.ElementAt(i));
                }
                }

            public void Draw()
            {
                foreach (Planete f in _planets)
                {
                    f.Draw();                    

                }
            }

            public void Update()
            {
                foreach (var item in _planets)
                {
                    item.Update();
                }
            }
                
            

        }

    }
}
