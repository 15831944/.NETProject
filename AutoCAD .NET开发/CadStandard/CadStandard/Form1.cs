﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using MySql.Data;
using MySql.Data.MySqlClient;

using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using AcadWnd = Autodesk.AutoCAD.Windows;
using Exception = System.Exception;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CadStandard
{
    public partial class Form1 : Form
    {
        public double pi = Math.PI;
        //人体参数
        public double FW = 42;         //前腰
        public double BW = 40;         //后腰
        public double S = 18;    //腰长
        public double FH = 50;   //前臀
        public double BH = 52;   //后臀
        public double L = 60;    //腿长
        public double K;    //膝长
        public double D = 18;    //直裆长
        //结构参数
        public double a = 60;    //裙长
        public double b = 3.5;   //腰头宽度
        public double c;         //腰头翘起量
        public double d;        //前腰松量
        public double e = 2;    //后臀松量
        public double f;        //后中下降量
        public double g;        //前臀松量
        public double h = 2;     //后腰松量
        public double y = 1;    //水平方向
        public double j;        //下摆起翘量
        public double o;        //后腰直线
        public double p;        //底摆直线
        //public double k;        //省道大小,计算得到
        public double i;        //省道长度
        public double m = 21;        //省道数量，计算得到
        public double n;        //省道总量，计算得到

        public string ServerName;
        public string Password;

        // 侧缝关键点
        public Point3d SilkFront = new Point3d(0, 0, 0);
        public Point3d SilkBehind = new Point3d(0, 0, 0);
        public Point3d SilkWave = new Point3d(0, 0, 0);
        public Point3d SilkCircle = new Point3d(0, 0, 0);
        public Point3d SilkHalfC = new Point3d(0, 0, 0);
        public Point3d SilkHead = new Point3d(0, 0, 0);
        // 底摆波浪模块丝缕关键点
        public Point3d SilkLapWave = new Point3d(0, 0, 0);
        // 底摆波浪关键点
        double r1 = 0;
        double r2 = 0;


        public Form1()
        {
            InitializeComponent();
            //textBox1.Text = "222";
            //textBox3.Text = "222";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox11.PasswordChar = '*';
        }

        public void setParameters()
        {

            this.FW = Convert.ToDouble(textBox1.Text);         //前腰
            this.BW = Convert.ToDouble(textBox2.Text);         //后腰
            this.S = Convert.ToDouble(textBox3.Text);    //腰长
            this.FH = Convert.ToDouble(textBox4.Text);   //前臀
            this.BH = Convert.ToDouble(textBox5.Text);   //后臀
            this.L = Convert.ToDouble(textBox6.Text);    //腿长
            this.K = Convert.ToDouble(textBox7.Text);    //膝长
            this.D = Convert.ToDouble(textBox8.Text);    //直裆长
        }


        // 生成样板按钮
        private void button1_Click(object sender, EventArgs e)
        {
            setParameters();
            if (checkBox2.Checked == true)
            {
                drawCircle();
            }

            if (checkBox3.Checked == true)
            {
                drawHalfCircle();
            }

            if (checkBox1.Checked == true)
            {
                drawWaveDress();
            }

            if (checkBox4.Checked == true)
            {
                drawFrontPiece();
                drawWaistHead();
                drawcloth();
            }

            //MessageBox.Show("success");

        }

        // 生成模块按钮
        private void button7_Click(object sender, EventArgs e)
        {

            if (checkBox6.Checked == true)
            {
                double r = (r1 + r2) * 2.0;
                drewLapWave(r);
            }

            if (checkBox5.Checked == true)
            {

                if (checkBox4.Checked == true)
                {
                    drawSilk(SilkFront, "front");
                    drawSilk(SilkBehind, "behind");
                    drawSilk(SilkHead, "head");
                }

                if (checkBox1.Checked == true)
                {
                    drawSilk(SilkWave, "wave");
                }
                if (checkBox2.Checked == true)
                {
                    drawSilk(SilkCircle, "wave");
                }
                if (checkBox3.Checked == true)
                {
                    drawSilk(SilkHalfC, "wave");
                }
                if (checkBox6.Checked == true)
                {
                    drawSilk(SilkLapWave, "wave");
                }
            }

            checkBox5.Checked = false;
            checkBox6.Checked = false;
        }

        // 数据库查询按钮
        private void button2_Click(object sender, EventArgs e)
        {

            string constr = "server=47.98.52.193;User Id=" + ServerName + ";password=" + Password + ";Database=autocloth";
            Console.WriteLine(constr);
            MySqlConnection mycon = new MySqlConnection(constr);

            try
            {
                mycon.Open();
                // cloth
                MySqlCommand mycmd = new MySqlCommand("SELECT * FROM bodydata WHERE bodydataId=" + textBox9.Text + ";", mycon);

                MySqlDataReader myDataReader = mycmd.ExecuteReader();
                if (myDataReader.HasRows == false)
                {
                    MessageBox.Show("无此项数据");
                }
                while (myDataReader.Read())
                {
                    textBox1.Text = Convert.ToString(myDataReader.GetInt32("frontWaist"));
                    textBox2.Text = Convert.ToString(myDataReader.GetInt32("frontButtock"));
                    textBox3.Text = Convert.ToString(myDataReader.GetInt32("kneeLength"));
                    textBox4.Text = Convert.ToString(myDataReader.GetInt32("legLength"));
                    textBox5.Text = Convert.ToString(myDataReader.GetInt32("waistLength"));
                    textBox6.Text = Convert.ToString(myDataReader.GetInt32("backButtock"));
                    textBox7.Text = Convert.ToString(myDataReader.GetInt32("backWaist"));
                    textBox8.Text = Convert.ToString(myDataReader.GetInt32("backWaist"));
                    /*
                     textBox1.Text = Convert.ToString(myDataReader.GetInt32("FW"));
                    textBox2.Text = Convert.ToString(myDataReader.GetInt32("BW"));
                    textBox3.Text = Convert.ToString(myDataReader.GetInt32("S"));
                    textBox4.Text = Convert.ToString(myDataReader.GetInt32("FH"));
                    textBox5.Text = Convert.ToString(myDataReader.GetInt32("BH"));
                    textBox6.Text = Convert.ToString(myDataReader.GetInt32("L"));
                    textBox7.Text = Convert.ToString(myDataReader.GetInt32("K"));
                    textBox8.Text = Convert.ToString(myDataReader.GetInt32("D"));
                     */
                }
                myDataReader.Close();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
                MessageBox.Show("连接失败");
            }
            finally
            {
                mycon.Close();
            }

        }

        // 清屏按钮
        private void button4_Click(object sender, EventArgs e)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            //doc.SendStringToExecute("AI_SELLALL\nERASE\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ERASE\nALL\n", true, false, true);
            doc.SendStringToExecute("\n", true, false, true);

            this.checkBox1.Checked = false;
            this.checkBox2.Checked = false;
            this.checkBox3.Checked = false;
            this.checkBox4.Checked = false;
            this.checkBox5.Checked = false;
            this.checkBox6.Checked = false;

        }

        // 保存按钮
        private void button5_Click(object sender, EventArgs e)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("SAVE\n", true, false, true);
        }

        // 连接数据库按钮
        private void button3_Click(object sender, EventArgs e)
        {
            ServerName = textBox10.Text;
            Password = textBox11.Text;
            string constr = "server=localhost;User Id=" + ServerName + ";password=" + Password + ";Database=autocloth";
            MySqlConnection mycon = new MySqlConnection(constr);
            try
            {
                mycon.Open();
                MessageBox.Show("连接成功，请进行下一步动作");
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
                MessageBox.Show("连接失败");
                Password = null;
                ServerName = null;
            }
            finally
            {
                mycon.Close();
                textBox11.Text = null;
            }
        }

        // 定位按钮
        private void button6_Click(object sender, EventArgs e)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ZOOM E\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
            doc.SendStringToExecute("ENTER\n", true, false, true);
        }

        // 画丝缕
        public void drawSilk(Point3d A, string type)
        {
            Database db = HostApplicationServices.WorkingDatabase;

            if (type.ToLower().Equals("front"))
            {
                Point3d down = new Point3d(A.X - 5, A.Y + 10, 0);
                Point3d up = new Point3d(A.X - 5, A.Y + 50, 0);
                Line Linefront = new Line(down, up);

                Point3d arr1 = new Point3d(down.X - 2, down.Y + 4, 0);
                Line arrow1 = new Line(down, arr1);
                Point3d arr2 = new Point3d(up.X + 2, up.Y - 4, 0);
                Line arrow2 = new Line(up, arr2);
                DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    //以写的方式打开模型空间块表记录
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    db.AddToModelSpace(Linefront, arrow2, arrow1);
                    //提交事务
                    trans.Commit();
                }
                docLock.Dispose();
            }
            else if (type.ToLower().Equals("head"))
            {
                Point3d left = new Point3d(A.X + 10, A.Y + 2, 0);
                Point3d right = new Point3d(A.X + 55, A.Y + 2, 0);
                Line Linehead = new Line(left, right);

                Point3d arr1 = new Point3d(left.X + 2, left.Y + 1, 0);
                Line arrow1 = new Line(left, arr1);
                Point3d arr2 = new Point3d(right.X - 2, right.Y - 1, 0);
                Line arrow2 = new Line(right, arr2);

                DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    //以写的方式打开模型空间块表记录
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    db.AddToModelSpace(Linehead, arrow2, arrow1);
                    //提交事务
                    trans.Commit();
                }
                docLock.Dispose();
            }
            else if (type.ToLower().Equals("behind"))
            {
                Point3d down = new Point3d(A.X + 5, A.Y + 10, 0);
                Point3d up = new Point3d(A.X + 5, A.Y + 50, 0);
                Line Linebehind = new Line(down, up);

                Point3d arr1 = new Point3d(down.X - 2, down.Y + 4, 0);
                Line arrow1 = new Line(down, arr1);
                Point3d arr2 = new Point3d(up.X + 2, up.Y - 4, 0);
                Line arrow2 = new Line(up, arr2);

                DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    //以写的方式打开模型空间块表记录
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    db.AddToModelSpace(Linebehind, arrow2, arrow1);
                    //提交事务
                    trans.Commit();
                }
                docLock.Dispose();
            }
            else if (type.ToLower().Equals("wave"))
            {
                Point3d begin = new Point3d(A.X + 20, A.Y - 5, 0);
                Point3d end = new Point3d(A.X + 50, A.Y - 5, 0);
                Line Linewave = new Line(begin, end);
                Point3d arr1 = new Point3d(begin.X + 4, begin.Y + 2, 0);
                Line arrow1 = new Line(begin, arr1);
                Point3d arr2 = new Point3d(end.X - 4, end.Y - 2, 0);
                Line arrow2 = new Line(end, arr2);
                DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    //以写的方式打开模型空间块表记录
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    db.AddToModelSpace(Linewave, arrow2, arrow1);
                    //提交事务
                    trans.Commit();
                }
                docLock.Dispose();
            }
            else
            {
                ;
            }
        }

        // 画底摆波浪
        public void drewLapWave(double r)
        {
            //获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            //获取命令行对象
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            //整圆裙
            double radius = (7.0 / 10.0 * L) + (FW + BW) / 2 / Math.PI;
            Point3d O = new Point3d(0, 350, 0);
            double OA = r / 2 / Math.PI;
            Arc Circle1 = new Arc(O, OA, 0, Math.PI * 2 - 0.0001);
            Arc Circle0 = new Arc(O, radius, 0, Math.PI * 2 - 0.0001);



            //Ellipse ellipse = new Ellipse(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), new Vector3d(100, 0, 0), 0.6,0,Math.PI*2);
            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            //定义一个指向当前数据库的事物处理
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                db.AddToModelSpace(Circle1, Circle0);
                trans.Commit();

            }
            docLock.Dispose();

            SilkLapWave = O;
        }

        //[CommandMethod("drawBehindPiece")]
        public void drawcloth()
        {
            //setParameters(double FW, double BW, double S, double FH, double BH, double L, double K, double D, double a, double b, double c, double d, double e, double f, double g, double h, double y, double j, double o, double p, double k, double i, double m, double n);
            //获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            //获取命令行对象
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            Point3d APoint = new Point3d(0, a - b, 0);
            Point3d BPoint = new Point3d(0, 0, 0);
            Line lineAB = new Line(APoint, BPoint);
            Point3d MPoint = new Point3d((BH + e) / 2, a - b, 0);
            Point3d EPoint = new Point3d(0, a - b - S, 0);
            Line lineAM = new Line(APoint, MPoint);
            Point3d SPoint = new Point3d(MPoint.X, MPoint.Y - S, 0);
            Line lineES = new Line(EPoint, SPoint);
            Point3d NPoint = new Point3d(MPoint.X, 0, 0);
            Line lineMN = new Line(MPoint, NPoint);
            Line lineBN = new Line(BPoint, NPoint);
            Point3d CPoint = new Point3d();

            Point3d HPoint = new Point3d(SPoint.X, SPoint.Y - 10, 0);
            Point3d I1Point = new Point3d(HPoint.X - y, HPoint.Y, 0);
            Point3d I2Point = new Point3d(HPoint.X + y, HPoint.Y, 0);

            Point3d WPoint = GetIntersection(I2Point, SPoint, APoint, MPoint);
            Point3d X_Point = GetIntersection(I2Point, SPoint, BPoint, NPoint);
            Line lineWX_ = new Line(WPoint, X_Point);

            n = (BH + e) / 2 - (BW + h) / 2;
            double dieta = ((BH + e) / 2 - (BW + h) / 2) / ((m % 10) + 1);
            if (dieta > 3 || dieta < 2)
            {
                ed.WriteMessage("请调整省道大小");
            }

            //侧缝
            Point3d Z_Point = new Point3d(WPoint.X - dieta, WPoint.Y, 0);
            Point3d G_Point = new Point3d(WPoint.X + 2 * (SPoint.X - WPoint.X) / 3, SPoint.Y + (WPoint.Y - SPoint.Y) / 3, 0);
            Polyline polyCe = getArcByBulge(G_Point, Z_Point, 0.3);
            Point3d Z__Point = getPointByExtend(Z_Point, polyCe.GetFirstDerivative(polyCe.EndPoint), 0.7);
            Line lineZ_Z__ = new Line(Z_Point, Z__Point);
            //画腰线
            Point3d A_Point = new Point3d(APoint.X, APoint.Y - 1, APoint.Z);
            Line lineZ__P = GetLineByVectorAndDot(Z__Point, lineZ_Z__.GetFirstDerivative(Z__Point).GetPerpendicularVector());
            Point3d PPoint = GetIntersection(A_Point, new Point3d(A_Point.X + 1, A_Point.Y, A_Point.Z), lineZ__P.StartPoint, lineZ__P.EndPoint);
            lineZ__P = new Line(Z__Point, PPoint);
            Point3d OPoint = new Point3d(A_Point.X + (BW + e) / 6, A_Point.Y, A_Point.Z);
            Point3d QPoint = lineZ__P.GetPointAtDist(lineZ__P.Length / 2);
            //Polyline polyYao = getArcByBulge(OPoint, QPoint, 0.1);
            Line lineA_O = new Line(A_Point, OPoint);
            Line lineQZ__ = new Line(QPoint, Z__Point);
            Polyline polyYao = getArcByBulge(A_Point, Z__Point, 0.2);
            //省道
            Point3d UPoint = polyYao.GetPointAtDist(polyYao.Length / 2);
            Point3d GPoint = getPointByExtend(UPoint, polyYao.GetFirstDerivative(UPoint).GetPerpendicularVector().RotateBy(Math.PI, polyYao.GetFirstDerivative(UPoint)), 11.0);
            Point3d U_Point = polyYao.GetPointAtDist(polyYao.Length / 2 - dieta / 2);
            Point3d U__Point = polyYao.GetPointAtDist(polyYao.Length / 2 + dieta / 2);
            Line lineUG = new Line(UPoint, GPoint);
            Line lineU_G = new Line(U_Point, GPoint);
            Line lineU__G = new Line(U__Point, GPoint);
            //转省
            Point3d ZhuanPoint = getPointByExtend(UPoint, lineUG.GetFirstDerivative(UPoint).GetPerpendicularVector().GetPerpendicularVector(), 1.0);
            Polyline polyYao1 = getArcByBulge(A_Point, ZhuanPoint, 0.1);
            Polyline polyYao2 = getArcByBulge(ZhuanPoint, Z__Point, 0.1);
            Point3dCollection intersect1 = new Point3dCollection();
            Point3dCollection intersect2 = new Point3dCollection();
            lineU_G.IntersectWith(polyYao1, Intersect.ExtendThis, intersect1, IntPtr.Zero, IntPtr.Zero);
            lineU__G.IntersectWith(polyYao2, Intersect.ExtendThis, intersect2, IntPtr.Zero, IntPtr.Zero);
            Line lineU_1 = new Line(U_Point, intersect1[0]);
            Line lineU__1 = new Line(U__Point, intersect2[0]);
            Line lineGZhuan = new Line(GPoint, ZhuanPoint);
            Line lineGG_ = new Line(GPoint, new Point3d(GPoint.X, 0, 0));

            //画下摆
            CPoint = GetIntersection(new Point3d(BPoint.X, BPoint.Y + 2, BPoint.Z), new Point3d(BPoint.X + 1, BPoint.Y + 2, BPoint.Z), WPoint, SPoint);
            Line lineCT = GetLineByVectorAndDot(CPoint, lineWX_.GetFirstDerivative(CPoint));
            Point3d TPoint = GetIntersection(lineCT.StartPoint, lineCT.EndPoint, lineBN.StartPoint, lineBN.EndPoint);
            lineCT = new Line(CPoint, TPoint);
            Point3d RPoint = new Point3d((BH + h) / 6, 0, 0);
            Point3d VPoint = lineCT.GetPointAtDist(lineCT.Length / 2);
            Polyline polyDownLap = getArcByBulge(RPoint, VPoint, 0.05);
            //定义一个指向当前数据库的事物处理


            r1 = new Line(CPoint, VPoint).Length + new Line(BPoint, RPoint).Length + polyDownLap.Length;

            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //以读的方式打开块表
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                //以写的方式打开模型空间块表记录
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                //增加实体
                db.AddToModelSpace(lineAB, lineAM, lineES, lineMN, lineBN, lineWX_, polyCe, lineZ_Z__);
                db.AddToModelSpace(polyYao);
                //
                //db.AddToModelSpace(lineA_O, lineQZ__);
                //省道
                db.AddToModelSpace(lineUG, lineU_G, lineU__G);
                db.AddToModelSpace(lineU_1, lineU__1, lineGZhuan);
                db.AddToModelSpace(lineGG_);
                //转省
                db.AddToModelSpace(polyYao1, polyYao2);
                //下摆
                db.AddToModelSpace(polyDownLap);
                //提交事务
                trans.Commit();
            }
            docLock.Dispose();

            SilkBehind = BPoint;
        }

        //[CommandMethod("drawFrontPiece")]
        public void drawFrontPiece()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;
            //前片定框架


            Point3d BPoint = new Point3d(60, 0, 0); //前面的原点
            Point3d APoint = new Point3d(BPoint.X, a - b, BPoint.Z);
            Line lineBA = new Line(BPoint, APoint);
            Point3d D_Point = new Point3d(APoint.X - (FH + g) / 2, APoint.Y, APoint.Z);
            Line lineAD_ = new Line(APoint, D_Point);
            Point3d C_Point = new Point3d(D_Point.X, BPoint.Y, D_Point.Z);
            Line lineD_C_ = new Line(D_Point, C_Point);
            Line lineBC_ = new Line(BPoint, C_Point);
            Point3d S_Point = new Point3d(APoint.X, APoint.Y - S, APoint.Z);
            Point3d SPoint = new Point3d(D_Point.X, S_Point.Y, S_Point.Z);
            Line lineSS_ = new Line(S_Point, SPoint);
            Point3d MPoint = new Point3d(SPoint.X - y, SPoint.Y - 10, SPoint.Z);
            Line lineSM = new Line(SPoint, MPoint);
            Point3d D__Point = GetIntersection(lineSM.StartPoint, lineSM.EndPoint, lineAD_.StartPoint, lineAD_.EndPoint);
            Point3d extPoint = GetIntersection(lineSM.StartPoint, lineSM.EndPoint, lineBC_.StartPoint, lineBC_.EndPoint);
            Line lineD__ext = new Line(D__Point, extPoint);
            //计算省道量
            n = (BH + e) / 2 - (BW + h) / 2;
            double dieta = ((FH + g) / 2 - (FW + d) / 2) / ((m / 10) / 2 + 1);
            if (dieta > 3 || dieta < 2)
            {
                ed.WriteMessage("请调整省道大小");
            }
            //侧缝
            Point3d D___Point = new Point3d(D__Point.X + dieta, D__Point.Y, D__Point.Z);
            Line lineSD__ = new Line(SPoint, D__Point);
            Point3d NPoint = lineSD__.GetPointAtDist(lineSD__.Length / 3);
            Polyline polyCeFeng = getArcByBulge(D___Point, NPoint, 0.3);
            Point3d DPoint = getPointByExtend(D___Point, polyCeFeng.GetFirstDerivative(polyCeFeng.StartPoint).GetPerpendicularVector().GetPerpendicularVector(), 0.5);
            Line lineD___D = new Line(D___Point, DPoint);
            //腰线
            Polyline polyWaist = getArcByBulge(DPoint, APoint, 0.01);
            //省道
            int numOfDart = (int)m / 10;
            Point3d UPoint = polyWaist.GetPointAtDist(polyWaist.Length / 3);

            Point3d GPoint = getPointByExtend(UPoint, polyWaist.GetFirstDerivative(UPoint).RotateBy(Math.PI, new Vector3d(0, 0, 1)).GetPerpendicularVector(), 9.0);
            Point3d EPoint = polyWaist.GetPointAtDist(polyWaist.Length / 3 - dieta / 2);
            Point3d FPoint = polyWaist.GetPointAtDist(polyWaist.Length / 3 + dieta / 2);

            Line lineUG = new Line(UPoint, GPoint);
            Line lineGE = new Line(GPoint, EPoint);
            Line lineGF = new Line(GPoint, FPoint);
            Point3d U_Point = getPointByExtend(UPoint, lineUG.GetFirstDerivative(UPoint).GetPerpendicularVector().GetPerpendicularVector(), 0.5);

            Line lineUU_ = new Line(UPoint, U_Point);

            Polyline polyWaist1 = getArcByBulge(U_Point, APoint, 0.07);

            Polyline polyWaist2 = getArcByBulge(DPoint, U_Point, 0.07);
            Point3dCollection intersect1 = new Point3dCollection();
            Point3dCollection intersect2 = new Point3dCollection();
            lineGE.IntersectWith(polyWaist2, Intersect.ExtendThis, intersect1, IntPtr.Zero, IntPtr.Zero);
            lineGF.IntersectWith(polyWaist1, Intersect.ExtendThis, intersect2, IntPtr.Zero, IntPtr.Zero);
            Line lineE_ = new Line(EPoint, intersect1[0]);
            Line lineF_ = new Line(FPoint, intersect2[0]);

            Line lineEE_ = new Line(EPoint, intersect1[0]);
            Line lineFF_ = new Line(FPoint, intersect2[0]);
            Line lineGH = new Line(GPoint, new Point3d(GPoint.X, 0, 0));
            //下摆
            Point3d CPoint = GetIntersection(new Point3d(BPoint.X, BPoint.Y + 2, BPoint.Z), new Point3d(BPoint.X - 1, BPoint.Y + 2, BPoint.Z), lineSM.StartPoint, lineSM.EndPoint);
            Line lineCT = GetLineByVectorAndDot(CPoint, lineD__ext.GetFirstDerivative(CPoint));
            Point3d TPoint = GetIntersection(lineCT.StartPoint, lineCT.EndPoint, lineBC_.StartPoint, lineBC_.EndPoint);
            lineCT = new Line(CPoint, TPoint);
            Point3d RPoint = new Point3d(BPoint.X - (BH + h) / 6, 0, 0);
            Point3d VPoint = lineCT.GetPointAtDist(lineCT.Length / 2);
            Polyline polyDownLap = getArcByBulge(VPoint, RPoint, 0.05);
            Line lineCV = new Line(CPoint, VPoint);
            //定义一个指向当前数据库的事物处理

            r2 = new Line(BPoint, RPoint).Length + new Line(CPoint, VPoint).Length + polyDownLap.Length;


            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                db.AddToModelSpace(lineBA, lineAD_, lineD_C_, lineBC_, lineSS_, lineD__ext);
                db.AddToModelSpace(polyCeFeng, lineD___D);
                db.AddToModelSpace(lineUG, lineUU_, polyWaist1, polyWaist2, lineE_, lineF_, lineGH, lineGE, lineGF, lineCV);
                db.AddToModelSpace(polyDownLap);
                trans.Commit();
            }
            docLock.Dispose();

            SilkFront = BPoint;
        }

        //[CommandMethod("drawWaistHead")]
        public void drawWaistHead()
        {
            //获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            //获取命令行对象
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;
            double yaochang = FW + BW + 3;
            double yaotouWidth = b;
            Point3d APoint = new Point3d(0, 60, 0);
            Point3d BPoint = new Point3d(yaochang, APoint.Y, APoint.Z);
            Point3d CPoint = new Point3d(BPoint.X, BPoint.Y + yaotouWidth, BPoint.Z);
            Point3d DPoint = new Point3d(APoint.X, CPoint.Y, CPoint.Z);
            Line lineAB = new Line(APoint, BPoint);
            Line lineCD = new Line(CPoint, DPoint);
            Line lineAD = new Line(APoint, DPoint);
            Line lineBC = new Line(BPoint, CPoint);

            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                db.AddToModelSpace(lineAB, lineCD, lineAD, lineBC);
                trans.Commit();
            }
            docLock.Dispose();

            SilkHead = APoint;
        }

        public void drawHalfCircle()
        {
            //获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            //获取命令行对象
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            //半圆裙
            double radius1 = (7.0 / 10.0 * L) + (FW + BW) / Math.PI;
            Point3d O1 = new Point3d(150, 350, 0);
            double O1A1 = (FW + BW) / Math.PI;
            Arc Circle11 = new Arc(O1, O1A1, Math.PI, Math.PI * 2);
            Arc Circle01 = new Arc(O1, radius1, Math.PI, Math.PI * 2);
            Point3d A_ = new Point3d(O1.X - O1A1, O1.Y, 0);
            Point3d B_ = new Point3d(O1.X + O1A1, O1.Y, 0);
            Point3d C_ = new Point3d(O1.X - radius1, O1.Y, 0);
            Point3d D_ = new Point3d(O1.X + radius1, O1.Y, 0);
            Line line1 = new Line(A_, C_);
            Line line2 = new Line(B_, D_);


            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            //定义一个指向当前数据库的事物处理
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                db.AddToModelSpace(Circle01, Circle11, line1, line2);
                trans.Commit();

            }
            docLock.Dispose();

            SilkHalfC = O1;
        }

        //[CommandMethod("drawCircle")]
        public void drawCircle()
        {
            //获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            //获取命令行对象
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            //整圆裙
            double radius = (7.0 / 10.0 * L) + (FW + BW) / 2 / Math.PI;
            Point3d O = new Point3d(0, 350, 0);
            double OA = (FW + BW) / 2 / Math.PI;
            Arc Circle1 = new Arc(O, OA, 0, Math.PI * 2 - 0.0001);
            Arc Circle0 = new Arc(O, radius, 0, Math.PI * 2 - 0.0001);

            //Ellipse ellipse = new Ellipse(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), new Vector3d(100, 0, 0), 0.6,0,Math.PI*2);
            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            //定义一个指向当前数据库的事物处理
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                db.AddToModelSpace(Circle1, Circle0);
                trans.Commit();

            }
            docLock.Dispose();

            SilkCircle = O;
        }

        //[CommandMethod("drawWaveDress")]
        public void drawWaveDress()
        {
            FW = 44;
            BW = 42;
            L = 101;
            D = 18;
            //获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            //获取命令行对象
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            //180°大圆
            Point3d O = new Point3d(0, 250, 0);
            double OA = (10 * Math.PI * D + 120 * Math.PI + 10 * (FW + BW) + 7 * Math.PI * L) / (20 * Math.PI);
            Arc arc1 = new Arc(O, OA, Math.PI, 2 * Math.PI);
            //225°小圆
            double r = (FW + BW) / (2 * Math.PI);
            double O_A = D + 12.0 + (FW + BW) / (2 * Math.PI);
            double O_O = OA - O_A;
            Point3d O_ = new Point3d(-O_O, 250, 0);
            Arc arc2 = new Arc(O_, r, Math.PI * 0.75, 1.5 * Math.PI);
            //腰线
            double O_K_ = r - 1;
            Point3d K_ = new Point3d(O_.X + O_K_, O.Y, 0);
            double Radius_radio = O_K_ / r;
            Ellipse ellipse = new Ellipse
                (O_, new Vector3d(0, 0, 1), new Vector3d(0, r, 0), Radius_radio, Math.PI * 1, Math.PI * 1.5);
            //裙摆
            Point3d Point_D = new Point3d(O_.X - r * Math.Sin(pi * 0.25), O_.Y + r * Math.Sin(pi * 0.25), 0);
            Point3d Point_D_ = new Point3d(Point_D.X - D * Math.Cos(pi / 9.0), Point_D.Y + D * Math.Sin(pi / 9.0), 0);
            Line D_D = new Line(Point_D, Point_D_);

            Point3d A = new Point3d(-OA, 250, 0);
            double k_AD_ = getVerticalBisector(Point_D_, A);
            Point3d middleAD_ = getMiddlePoint(Point_D_, A);
            Point3d inter1 = GetIntersection
                (A, new Point3d(-OA + 50, 250, 0), middleAD_, new Point3d(middleAD_.X + 1, middleAD_.Y + k_AD_, 0));

            Arc arc3 = new Arc(inter1, OA - Math.Abs(inter1.X), Math.Atan((inter1.Y - Point_D_.Y) / (inter1.X - Point_D_.X)), pi);

            //double k_D_D = Math.Tan(pi * 7.0 / 18.0);
            //Point3d inter1 = GetIntersection
            //    (A, new Point3d(-OA + 50, 250, 0), Point_D_, new Point3d(Point_D_.X + 1, Point_D_.Y + k_D_D, 0));
            //Arc arc3 = new Arc(inter1, OA - Math.Abs(inter1.X), pi * 7.0 / 18.0, pi);

            Point3d E_ = new Point3d(-Math.Sqrt(2) / 2.0 * (OA - 1.0), 250.0 - Math.Sqrt(2) / 2.0 * (OA - 1.0), 0);
            double k_AE_ = getVerticalBisector(E_, A);
            Point3d middleAE_ = getMiddlePoint(E_, A);
            Point3d inter2 = GetIntersection
                (A, new Point3d(-OA + 50, 250, 0), middleAE_, new Point3d(middleAE_.X + 1, middleAE_.Y + k_AE_, 0));
            Arc arc4 = new Arc(inter2, OA - Math.Abs(inter2.X), pi, pi + Math.Atan((inter2.Y - E_.Y) / (inter2.X - E_.X)));

            Point3d B = new Point3d(0, 250 - OA, 0);
            double k_BE_ = getVerticalBisector(B, E_);
            Point3d middleBE_ = getMiddlePoint(B, E_);
            Point3d inter3 = GetIntersection
                (B, new Point3d(0, 200, 0), middleBE_, new Point3d(middleBE_.X + 1, middleBE_.Y + k_BE_, 0));
            Arc arc5 = new Arc(inter3, OA - O.Y + inter3.Y, pi + Math.Atan((inter3.Y - E_.Y) / (inter3.X - E_.X)), pi * 1.5);

            Line OE_ = new Line(O, E_);

            Point3d F_ = new Point3d(Math.Sqrt(2) / 2.0 * (OA - 1.0), 250.0 - Math.Sqrt(2) / 2.0 * (OA - 1.0), 0);
            double k_BF_ = getVerticalBisector(B, F_);
            Point3d middleBF_ = getMiddlePoint(B, F_);
            Point3d inter4 = GetIntersection
                (B, new Point3d(0, 200, 0), middleBF_, new Point3d(middleBF_.X + 1, middleBF_.Y + k_BF_, 0));
            Arc arc6 = new Arc(inter4, OA - O.Y + inter4.Y, pi * 1.5, 2 * pi + Math.Atan((inter4.Y - F_.Y) / (inter4.X - F_.X)));

            Point3d C = new Point3d(OA, 250, 0);
            double k_CF_ = getVerticalBisector(C, F_);
            Point3d middleCF_ = getMiddlePoint(C, F_);
            Point3d inter5 = GetIntersection
                (C, new Point3d(OA + 50, 250, 0), middleCF_, new Point3d(middleCF_.X + 1, middleCF_.Y + k_CF_, 0));
            Arc arc7 = new Arc(inter5, OA - Math.Abs(inter5.X), 2 * pi + Math.Atan((inter5.Y - F_.Y) / (inter5.X - F_.X)), pi * 2.0);

            Line OF_ = new Line(O, F_);
            Line CK_ = new Line(C, K_);

            DocumentLock docLock = AcadApp.DocumentManager.MdiActiveDocument.LockDocument();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                //db.AddToModelSpace(arc1, arc2, ellipse, D_D, arc3, arc4, arc5, arc6, arc7, OE_, OF_, CK_);
                db.AddToModelSpace(arc2, ellipse, D_D, arc3, arc4, arc5, arc6, arc7, CK_);
                trans.Commit();
            }
            docLock.Dispose();

            SilkWave = O;
        }

        //根据凸度画圆弧
        public Polyline getArcByBulge(Point3d pointStart, Point3d pointEnd, double bulge)
        {
            double sita = 4 * Math.Atan(bulge);
            double LAB = Math.Sqrt(Math.Pow((pointEnd.X - pointStart.X), 2) + Math.Pow((pointEnd.Y - pointStart.Y), 2));
            double R = LAB / Math.Sin(sita / 2);
            double kl = (pointEnd.X - pointStart.X) / (pointStart.Y - pointEnd.Y);
            double m = (Math.Pow(pointStart.X, 2) - Math.Pow(pointEnd.X, 2) + Math.Pow(pointStart.Y, 2) - Math.Pow(pointEnd.Y, 2)) / (2 * pointStart.Y - 2 * pointEnd.Y);
            double a = 1 + Math.Pow(kl, 2);
            double b = -2 * pointStart.X + 2 * kl * m - 2 * pointStart.Y * kl;
            double c = Math.Pow(pointStart.X, 2) + Math.Pow(pointStart.Y, 2) - 2 * m * pointStart.Y + Math.Pow(m, 2) - Math.Pow(R, 2);
            double dita = Math.Pow(b, 2) - 4 * a * c;
            Arc arc = new Arc();

            if ((pointEnd.Y < pointStart.Y && bulge > 0) || (pointEnd.Y > pointStart.Y && bulge < 0))
            {
                double centerX = (-1 * b + Math.Sqrt(dita)) / (2 * a);
                double centerY = kl * centerX + m;
                Point2d center = new Point2d(centerX, centerY);
                Vector2d vectorStart = new Point2d(pointStart.X, pointStart.Y) - new Point2d(center.X, center.Y);
                Vector2d vectorEnd = new Point2d(pointEnd.X, pointEnd.Y) - new Point2d(center.X, center.Y);
                double startAngle = vectorStart.Angle;
                double endAngle = vectorEnd.Angle;
                arc = new Arc(new Point3d(center.X, center.Y, 0), new Vector3d(0, 0, 1), R, startAngle, endAngle);
            }
            else if ((pointEnd.Y < pointStart.Y && bulge < 0) || (pointEnd.Y > pointStart.Y && bulge > 0))
            {
                double centerX = (-1 * b - Math.Sqrt(dita)) / (2 * a);
                double centerY = kl * centerX + m;
                Point2d center = new Point2d(centerX, centerY);
                Vector2d vectorStart = new Point2d(pointStart.X, pointStart.Y) - new Point2d(center.X, center.Y);
                Vector2d vectorEnd = new Point2d(pointEnd.X, pointEnd.Y) - new Point2d(center.X, center.Y);
                double startAngle = vectorStart.Angle;
                double endAngle = vectorEnd.Angle;
                arc = new Arc(new Point3d(center.X, center.Y, 0), new Vector3d(0, 0, 1), R, startAngle, endAngle);
            }
            else if ((pointEnd.Y == pointStart.Y && pointStart.X > pointEnd.X && bulge > 0) || (pointEnd.Y == pointStart.Y && pointStart.X < pointEnd.X && bulge < 0))
            {
                double centerX = (-1 * b) / (2 * a);
                double centerY = pointStart.Y - Math.Sqrt(Math.Pow(R, 2) - Math.Pow(centerX - pointStart.X, 2));
                Point2d center = new Point2d(centerX, centerY);
                Vector2d vectorStart = new Point2d(pointStart.X, pointStart.Y) - new Point2d(center.X, center.Y);
                Vector2d vectorEnd = new Point2d(pointEnd.X, pointEnd.Y) - new Point2d(center.X, center.Y);
                double startAngle = vectorStart.Angle;
                double endAngle = vectorEnd.Angle;
                arc = new Arc(new Point3d(center.X, center.Y, 0), new Vector3d(0, 0, 1), R, startAngle, endAngle);
            }
            else if ((pointEnd.Y == pointStart.Y && pointStart.X < pointEnd.X && bulge > 0) || (pointEnd.Y == pointStart.Y && pointStart.X > pointEnd.X && bulge < 0))
            {
                double centerX = (-1 * b) / (2 * a);
                double centerY = pointStart.Y + Math.Sqrt(Math.Pow(R, 2) - Math.Pow(centerX - pointStart.X, 2));
                Point2d center = new Point2d(centerX, centerY);
                Vector2d vectorStart = new Point2d(pointStart.X, pointStart.Y) - new Point2d(center.X, center.Y);
                Vector2d vectorEnd = new Point2d(pointEnd.X, pointEnd.Y) - new Point2d(center.X, center.Y);
                double startAngle = vectorStart.Angle;
                double endAngle = vectorEnd.Angle;
                arc = new Arc(new Point3d(center.X, center.Y, 0), new Vector3d(0, 0, 1), R, startAngle, endAngle);
            }
            Point2dCollection pointSet = new Point2dCollection();
            double length = arc.Length;
            for (int i = 0; i <= 100; i++)
            {
                pointSet.Add(arc.GetPointAtDist(length * i / 100).Point3To2());
            }
            Polyline poly = new Polyline();
            for (int i = 0; i <= 100; i++)
            {
                poly.AddVertexAt(i, pointSet[i], 0, 0, 0);
            }
            return poly;
        }


        //根据一点，一个向量求直线
        public Line GetLineByVectorAndDot(Point3d point, Vector3d vector)
        {
            double angle = new Vector2d(vector.X, vector.Y).Angle;
            Line line = new Line(point, new Point3d(point.X + Math.Cos(angle), point.Y + Math.Sin(angle), point.Z));
            return line;
        }

        //沿某向量方向延长一定长度的点
        public Point3d getPointByExtend(Point3d pointStart, Vector3d vector, double length)
        {
            double angle = new Vector2d(vector.X, vector.Y).Angle;
            double x = pointStart.X + length * Math.Cos(angle);
            double y = pointStart.Y + length * Math.Sin(angle);
            return new Point3d(x, y, 0);
        }

        //求直线斜率的角度
        public double Sita(Point3d point1, Point3d point2)
        {
            return Math.Atan((point1.Y - point2.Y) / (point1.X - point2.X));
        }

        //求直线斜率
        public double gradient(Point3d point1, Point3d point2)
        {
            return (point1.Y - point2.Y) / (point1.X - point2.X);
        }
        //获取两条直线的交点
        public Point3d GetIntersection(Point3d lineFirstStar, Point3d lineFirstEnd, Point3d lineSecondStar, Point3d lineSecondEnd)
        {
            double a = 0, b = 0;
            int state = 0;
            if (lineFirstStar.X != lineFirstEnd.X)
            {
                a = (lineFirstEnd.Y - lineFirstStar.Y) / (lineFirstEnd.X - lineFirstStar.X);
                state |= 1;
            }
            if (lineSecondStar.X != lineSecondEnd.X)
            {
                b = (lineSecondEnd.Y - lineSecondStar.Y) / (lineSecondEnd.X - lineSecondStar.X);
                state |= 2;
            }
            switch (state)
            {
                case 0: //L1与L2都平行Y轴
                    {
                        if (lineFirstStar.X == lineSecondStar.X)
                        {
                            //throw new Exception("两条直线互相重合，且平行于Y轴，无法计算交点。");
                            return new Point3d(0, 0, 0);
                        }
                        else
                        {
                            //throw new Exception("两条直线互相平行，且平行于Y轴，无法计算交点。");
                            return new Point3d(0, 0, 0);
                        }
                    }
                case 1: //L1存在斜率, L2平行Y轴
                    {
                        double x = lineSecondStar.X;
                        double y = (lineFirstStar.X - x) * (-a) + lineFirstStar.Y;
                        return new Point3d(x, y, 0);
                    }
                case 2: //L1 平行Y轴，L2存在斜率
                    {
                        double x = lineFirstStar.X;
                        //网上有相似代码的，这一处是错误的。你可以对比case 1 的逻辑 进行分析
                        //源code:lineSecondStar * x + lineSecondStar * lineSecondStar.X + p3.Y;
                        double y = (lineSecondStar.X - x) * (-b) + lineSecondStar.Y;
                        return new Point3d(x, y, 0);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (a == b)
                        {
                            // throw new Exception("两条直线平行或重合，无法计算交点。");
                            return new Point3d(0, 0, 0);
                        }
                        double x = (a * lineFirstStar.X - b * lineSecondStar.X - lineFirstStar.Y + lineSecondStar.Y) / (a - b);
                        double y = a * x - a * lineFirstStar.X + lineFirstStar.Y;
                        return new Point3d(x, y, 0);
                    }
            }
            // throw new Exception("不可能发生的情况");
            return new Point3d(0, 0, 0);
        }

        public Point2dCollection GetPointCollection(Point3d point1, Point3d point2, Point3d w)
        {
            Point2dCollection pts1 = new Point2dCollection();
            Point2dCollection pts3 = new Point2dCollection();
            //double gap = Math.Abs(point2.X - point1.X) / 60;
            double gap = Math.Abs(w.X - point1.X) / 20;
            double a = 0;   //斜率
            for (int i = 0; i <= 20; i++)
            {
                pts1.Add(new Point2d(point1.X + i * gap, point1.Y));
            }
            Point3d temp = new Point3d();
            temp = point2;
            for (int i = 19; i >= 0; i--)
            {
                a = (pts1[i].Y - temp.Y) / (pts1[i].X - temp.X);
                pts3.Add(new Point2d(temp.X - gap, temp.Y + gap * Math.Abs(a)));
                temp = new Point3d(temp.X - gap, temp.Y + gap * Math.Abs(a), 0);
            }
            pts3.Add(new Point2d(point1.X, point1.Y));
            //pts3.Add(new Point2d(point2.X, point2.Y));
            return pts3;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        // 求两个点的垂直平分线的斜率
        public double getVerticalBisector(Point3d A, Point3d B)
        {
            Point3d middlePoint = new Point3d((A.X + B.X) / 2.0, (A.Y + B.Y) / 2.0, 0);
            double k = (A.Y - B.Y) / (A.X - B.X);
            double k_ = -1.0 / k;
            return k_;
        }

        // 求两个点的终点
        public Point3d getMiddlePoint(Point3d A, Point3d B)
        {
            return new Point3d((A.X + B.X) / 2.0, (A.Y + B.Y) / 2.0, 0);
        }

     
    }

}
