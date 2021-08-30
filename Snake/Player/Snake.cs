using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Windows.Forms;
    public enum Direction
    {
        LEFT=0,
        UP=1,
        RIGHT=2,
        DOWN=3,
    }



namespace Snake.Player
{



    internal interface Movement
    {
        void MoveSnake(float speed);
        void MoveSnake(int speed);

        void initDir(float x, int y);

    }

   public class Snake
    {

       PointF head;
       float speed=1,speed2=0;
       float init;
       private PointF p;
       private Direction D= Direction.LEFT;

       


       public Snake()
       {
       }

       public void Turn(Direction d)
       {
           switch (d)
           {
               case Direction.LEFT:
                   break;

               case Direction.DOWN:
                   break;

               case Direction.UP:
                   break;

               case Direction.RIGHT:
                   break;
           }
       }

       private void left()
       {
           switch(D)
           {
               case Direction.DOWN:
                   p.X = 0;
                   p.Y = 16;
                   break;

               case Direction.UP:
                   p.X = 0;
                   p.Y = -16;
                   break;
           }
       }

       private void right()
       {
           switch (D)
           {
               case Direction.DOWN:
                                      p.X = 0;
                   p.Y = -16;
                   break;

               case Direction.UP:
                                      p.X = 0;
                   p.Y = 16;
                   break;
           }
       }

       private void up()
       {
           switch (D)
           {
               case Direction.RIGHT:
                   break;

               case Direction.LEFT:
                   break;
           }
       }

       private void down()
       {
           switch (D)
           {
               case Direction.RIGHT:
                   break;

               case Direction.LEFT:
                   break;
           }
       }


       public void DrawSnake(Graphics g)
       {

           g.FillRectangle(new SolidBrush(Color.Black), head.X, head.Y, 16, 16);

       }

       public PointF GetCords
       {
           get
           {
               return head;
           }

           set
           {
               head = value;
           }
       }

       public float Speed
       {
           get
           {
               return speed;
           }
           set
           {
               speed = value;
               p.X = speed;
           }
       }
       public float Speed2
       {
           get
           {
               return speed2;
           }
           set
           {
               speed2 = value;
           }
       }

       public void MoveSnake()
       {
           if (init >= 1)
           {
               head.X += p.X;
               head.Y += p.Y;
               init = 0;
           }
           else
           {
               init += speed;
           }
       }

       public bool Collision(PointF p)
       {
           return false;
       }


    }
}
