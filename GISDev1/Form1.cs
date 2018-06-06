using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GISDev1
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public string toolAction = "";   //该字符创全局变量用于存储当前用户选择要进行地图浏览操作的动作内容，拉框放大：“drag zoom in”、拉框缩小：“drag zoom out”、平移：“pan”
        public string str1 = "";
        public static string strCurrentBookMakName = "";
        public IFeatureLayer pGlobeFeatureLayer = null;     //定义一个全局变量，用于存储在TOCControl上进行鼠标点击时所选择的特征层
        public IMarkerSymbol pSelectedMarkSymbol = null;
        public ILineSymbol pSelectedLineSymbol = null;
        public IFillSymbol pSelectedFillSymbol = null;
        public IFeatureClass pGlobeFeatureClass = null;

        public Form1()
        {
            InitializeComponent();
        }



        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pOpenMXDDoc = new ControlsOpenDocCommand();
            pOpenMXDDoc.OnCreate(axMapControl1.Object);
            pOpenMXDDoc.OnClick();
            //添加地图文档
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pSaveMap = new ControlsSaveAsDocCommand();
            pSaveMap.OnCreate(axMapControl1.Object);

            pSaveMap.OnClick();
            //保存地图文档
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openDLG = new OpenFileDialog(); //创建打开文件对话框
            openDLG.Title = "open Shape File"; //  给对话框的标题赋值
            openDLG.Filter = "Shape File(*.shp)|*.shp";  // 设定对话框可以打开的文件类型，文件后缀为*.shp
            if (openDLG.ShowDialog() == DialogResult.OK)  // 打开对话框，若对话框操作选择文件完成后选择的是ok按钮，则执行下面的代码
            {
                string sFilePath = openDLG.FileName;   //   获取选择的文件的路径和名称
                int index = sFilePath.LastIndexOf("\\");  // 判定文件名字符串中的最后一个“\”符号的位置

                string sPath = sFilePath.Substring(0, index);   //截取文件路径
                string sFileName = sFilePath.Substring(index + 1);    //截取文件名称
                IWorkspaceFactory pWorkspaceFectory = new ShapefileWorkspaceFactory();   //创建能打开shapefile数据文件的工作空间工厂对象
                IWorkspace pWorkspace = pWorkspaceFectory.OpenFromFile(sPath, 0);   // 用工作空间工厂对象将shapefile文件所在的文件夹打开，作为工作空间对象
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;   // 将工作空间接口转换为 矢量特征工作空间接口
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(sFileName);   // 调用矢量特征工作空间接口的OpenFeatureClass方法打开shapefile矢量特征类，矢量特征类中存放的就是矢量数据
                ILayer pLayer = new FeatureLayer();    // 创建一个矢量特征层
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;   // 获得矢量特征层接口对象
                pFeatureLayer.FeatureClass = pFeatureClass;    //将矢量特征层的数据源指向刚打开的shapefile矢量特征类
                axMapControl1.Map.AddLayer(pLayer);    //将矢量特征层添加到地图控件的地图中
                axMapControl1.ActiveView.Refresh();    //刷新地图

            }
            //添加shape数据
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openDLG = new OpenFileDialog();  //创建打开文件对话框
            openDLG.Title = "open Shape File";  //  给对话框的标题赋值
            openDLG.Filter = "Raster File(*.*)|*.bmp;*.tif;*.jpg;*.img|(*.bmp)|*.bmp|(*.jpg)|*.jpg|(*.tif)|*.tif|(*.img)|*.img";   // 设定对话框可以打开的文件类型，文件后缀分别为*.bmp;*.tif;*.jpg;*.img
            if (openDLG.ShowDialog() == DialogResult.OK) // 打开对话框，若对话框操作选择文件完成后选择的是ok按钮，则执行下面的代码
            {
                string sFilePath = openDLG.FileName; //   获取选择的文件的路径和名称
                int index = sFilePath.LastIndexOf("\\"); // 判定文件名字符串中的最后一个“\”符号的位置

                string sPath = sFilePath.Substring(0, index);  //截取文件路径
                string sFileName = sFilePath.Substring(index + 1);   //截取文件名称
                IWorkspaceFactory pWorkspaceFectory = new RasterWorkspaceFactory();  //创建能打开栅格数据文件的工作空间工厂对象
                IWorkspace pWorkspace = pWorkspaceFectory.OpenFromFile(sPath, 0); // 用工作空间工厂对象将栅格文件所在的文件夹打开，作为工作空间对象
                IRasterWorkspace pRasterWorkspace = pWorkspace as IRasterWorkspace;  // 将工作空间接口转换为 栅格工作空间接口
                IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(sFileName); // 调用栅格工作空间接口的OpenRasterDataset方法打开指定的栅格数据集

                ILayer pLayer = new RasterLayer();   // 创建一个栅格层
                IRasterLayer pRasterLayer = pLayer as IRasterLayer;  // 获得栅格层接口对象
                pRasterLayer.CreateFromDataset(pRasterDataset);   //根据刚才打开的栅格数据集进行栅格图层的创建

                axMapControl1.Map.AddLayer(pLayer);   //将栅格层添加到地图控件的地图中
                axMapControl1.ActiveView.Refresh();   //刷新地图

            }
            //添加栅格数据
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FolderBrowserDialog openDLG = new FolderBrowserDialog();    //创建文件夹浏览对话框

            if (openDLG.ShowDialog() == DialogResult.OK)   //打开对话框，选择文件夹后，若点击ok按钮
            {
                string sFilePath = openDLG.SelectedPath;   //获得选择的文件夹

                IWorkspaceFactory pWorkspaceFectory = new FileGDBWorkspaceFactory();   // 创建能够打开文件型数据库的工作空间工厂对象
                IWorkspace pWorkspace = pWorkspaceFectory.OpenFromFile(sFilePath, 0);   //打开对应的文件夹，返回工作空间对象
                IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);   //获取文件数据库中的所有矢量特征数据集集合

                pEnumDataset.Reset();   //指向矢量特征数据集集合头部
                IDataset pDataset = pEnumDataset.Next();    //获取第一个矢量特征数据集
                while (pDataset != null)    //若矢量特征数据集非空
                {
                    if (pDataset is IFeatureDataset)    // 当前矢量特征数据集是否为FeatureDataset
                    {
                        IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;    //接口转换为IFeatureWorkspace
                        IFeatureDataset pFeatureDaset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);    //打开当前矢量特征数据集
                        IEnumDataset pSubEnumdataset = pFeatureDaset.Subsets;    //获取其子数据集
                        pSubEnumdataset.Reset();    //指向子数据集头部
                        IDataset pSubDataset = pSubEnumdataset.Next();    //获取子数据集的第一个数据集
                        while (pSubDataset != null && pSubDataset is IFeatureClass)   //当新获取的这个新数据集是FeatureClass
                        {
                            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pSubDataset.Name);     // 打开该IFeatureClass
                            IFeatureLayer pLayer = new FeatureLayer();    //创建一个矢量特征层
                            pLayer.FeatureClass = pFeatureClass;    //给该层数据源赋值
                            axMapControl1.Map.AddLayer(pLayer as ILayer);   //添加到地图控件中
                            pSubDataset = pSubEnumdataset.Next();   //获取下一个新的子数据集

                        }
                    }
                    else if (pDataset is IFeatureClass)   //当前矢量特征数据集是否为FeatureClass
                    {
                        IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;   //接口转换为IFeatureWorkspace
                        IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);   // 打开该IFeatureClass
                        IFeatureLayer pLayer = new FeatureLayer();   //创建一个矢量特征层
                        pLayer.FeatureClass = pFeatureClass;    //给该层数据源赋值
                        axMapControl1.Map.AddLayer(pLayer as ILayer);   //添加到地图控件中

                    }
                    pDataset = pEnumDataset.Next();   //获取下一个数据集
                }
                axMapControl1.ActiveView.Refresh();   //地图刷新

            }
            //添加GDB
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openDLG = new OpenFileDialog();   //创建打开文件对话框
            openDLG.Title = "open mdb database";    //  给对话框的标题赋值
            openDLG.Filter = "mdb File(*.*)|*.mdb";   // 设定对话框可以打开的文件类型，文件后缀为*.mdb，即access数据库
            if (openDLG.ShowDialog() == DialogResult.OK)  // 打开对话框，若对话框操作选择文件完成后选择的是ok按钮，则执行下面的代码
            {
                string sFilePath = openDLG.FileName;       //   获取选择的文件的路径和名称

                IWorkspaceFactory pWorkspaceFectory = new AccessWorkspaceFactory();  //创建能打开Access数据库的工作空间工厂对象
                IWorkspace pWorkspace = pWorkspaceFectory.OpenFromFile(sFilePath, 0);  // 用工作空间工厂对象将Access数据库打开，作为工作空间对象
                IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);  //获取所有矢量特征要素数据集

                pEnumDataset.Reset();   // 指向头部
                IDataset pDataset = pEnumDataset.Next();   //获得第一个数据集
                if (pDataset is IFeatureDataset)   // 判定是否为矢量特征要素数据集
                {
                    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;   //接口转换为矢量特征要素工作空间接口
                    IFeatureDataset pFeatureDaset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);   //打开指定名称的数据集
                    IEnumDataset pSubEnumdataset = pFeatureDaset.Subsets;   //获取打开数据集的子数据集对象
                    pSubEnumdataset.Reset();   // 
                    IDataset pSubDataset = pSubEnumdataset.Next();   //获取子数据集对象的第一个数据集
                    while (pSubDataset != null && pSubDataset is IFeatureClass)   // 子子数据集非空
                    {
                        IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pSubDataset.Name);   //打开指定名称的矢量要素特征类
                        IFeatureLayer pLayer = new FeatureLayer();   // 创建矢量特征层
                        pLayer.FeatureClass = pFeatureClass;    //  给矢量特征层的数据源附值为新打开的矢量要素特征类
                        axMapControl1.Map.AddLayer(pLayer as ILayer);   // 添加到地图中
                        pSubDataset = pSubEnumdataset.Next();   //获取下一个子数据集

                    }
                    axMapControl1.ActiveView.Refresh();   //刷新地图
                }
            }
            //添加access
        }

        private void axTOCControl2_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
      

            if (axMapControl1.LayerCount > 0)    //若地图中图层非0
            {

                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;   // 定义一个esri的toc控件的item对象
                IBasicMap pBasicmap = new Map() as IBasicMap;   // 定义一个基础map对象
                pGlobeFeatureLayer = new FeatureLayer();    // 定义一个特征层
                ILayer pLayer = pGlobeFeatureLayer as ILayer;    //接口查询

                object nuk = new object();   //
                object pIndex = new object();  //

                axTOCControl2.HitTest(e.x, e.y, ref pItem, ref pBasicmap, ref pLayer, ref nuk, ref pIndex);  //该方法用于检测当前鼠标所在的位置所对应的是什么类型的item，并把相应的对象返回
                pGlobeFeatureLayer = pLayer as IFeatureLayer;    // 重新给全局对象pGlobeFeatureLayer赋值，让其等于新获取到的图层

                if (e.button == 2)   //若单击鼠标右键
                {
                    contextMenuStrip1.Show(axTOCControl2, e.x, e.y);    //在TOC控件的当前鼠标位置弹出右键菜单
                }
            }

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayerDTForm1 frmTalbe = new LayerDTForm1();  //创建属性表显示的窗口对象
            frmTalbe.PFeatureLayer = pGlobeFeatureLayer;  //给窗口对象的特征层属性赋值
            frmTalbe.Show();  //打开窗口
        }

        private void 移除图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Map.DeleteLayer(pGlobeFeatureLayer as ILayer);  //删除当前选择到的图层
            axMapControl1.ActiveView.Refresh();
        }

        private void 缩放至图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.ActiveView.Extent = pGlobeFeatureLayer.AreaOfInterest; // 将选择到的图层的空间范围赋值给当前地图范围，实现本层的全部显示
            axMapControl1.ActiveView.Refresh();
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {

            for (int i = 0; i < axMapControl1.ActiveView.FocusMap.LayerCount; i++)    //根据axMapControl1（主地图窗口）中正处于打开状态的地图的图层数进行循环
            {
                axMapControl2.AddLayer(axMapControl1.ActiveView.FocusMap.get_Layer(i));   //利用索引窗口控件的Addlayer方法依次把主窗口中的图层添加索引窗口中
            }
            axMapControl2.Extent = axMapControl1.FullExtent; //将索引窗口的地图设为整图显示
            axMapControl2.Refresh(); //刷新索引窗口
        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {




            IEnvelope pEnvelop = e.newEnvelope as IEnvelope;   //将参数e转换成 Ienvelop接口对象，相当于把新的空间范围存放在了pEnvelop接口对象中了


            IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;   //获得索引窗口的图形要素容器，
            //该容器可以简单的理解为在索引地图的上面蒙上一层透明的层，
            //该层中可以绘制点、线、面、文本等多种图形要素

            pGraphicsContainer.DeleteAllElements();     // 删除图形要素容器中原来的所有要素

            IElement pElemet = new RectangleElement();    //创建一个矩形要素
            pElemet.Geometry = pEnvelop;    // 使该矩形要素的几何实体等于pEnvelop接口对象，相当于该矩形要素的空间范围等于新传进来的空间范围

            IRgbColor pColor = new RgbColor();   //创建一个RGB颜色对象，
            pColor.Red = 255;   // 红色值
            pColor.Green = 0;   //绿色值
            pColor.Blue = 0;    //蓝色值
            pColor.Transparency = 255;    // 该颜色对象的透明度 0-255，0为纯透明，255为不透明

            ILineSymbol pLine = new SimpleLineSymbol();    // 创建一个简单的线符号
            pLine.Color = pColor;   //给该线符号附上颜色，目前为红色
            pLine.Width = 3;    //线的宽度为3个像素

            pColor.Transparency = 0;   // 使颜色对象全透明

            IFillSymbol pFillsymbol = new SimpleFillSymbol();   //创建一个简单的填充符号
            pFillsymbol.Color = pColor;     // 让该填充符号纯透明，
            pFillsymbol.Outline = pLine;    //给该填充符号的边界附上一个红色的边框

            IFillShapeElement pFillshapeEle = pElemet as IFillShapeElement;// 将矩形要素pElemet转换成填充要素
            pFillshapeEle.Symbol = pFillsymbol;   //给填充要素的符号赋值为刚才创建的具有红色边框的填充符号


            pGraphicsContainer.AddElement((IElement)pFillshapeEle, 0);// 将新获得的具有红色边框的填充要素添加进索引窗口的图形要素容器中
            axMapControl2.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);  //让索引窗口的图形绘制内容刷新


        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1)  //判定鼠标左键是否按下
            {
                IPoint pPoint = new ESRI.ArcGIS.Geometry.Point(); // 创建一个点对象
                pPoint.PutCoords(e.mapX, e.mapY); // 给点对象的x，y赋值为鼠标所在位置的x，y坐标
                axMapControl1.CenterAt(pPoint);  // 将地图主窗口的中心点定位到新的位置
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null); //主窗口地图刷新

            }
            else if (e.button == 2)  //判定鼠标右键是否按下
            {
                IEnvelope pEnvlope = axMapControl2.TrackRectangle();// 调用axMapControl2的矩形框跟踪功能进行拉框操作，
                //将拉框完成后的范围赋值给一个Envelope对象
                //此处，你可以尝试axMapControl2.TrackPolygon()，axMapControl2.TrackCircle()，axMapControl2.TrackLine()等方法的调用，看有什么效果
                axMapControl1.Extent = pEnvlope;  //将新的拉框范围赋值给地图主窗口的空间范围
               
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null); //主窗口地图刷新


            }
        }

        private void axMapControl2_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.button == 1)  //判定鼠标左键是否按下
            {
                IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();  // 创建一个点对象
                pPoint.X = e.mapX;    // 给点对象的x赋值为鼠标所在位置的x坐标
                pPoint.Y = e.mapY;    //给点对象的y赋值为鼠标所在位置的y坐标
                axMapControl1.CenterAt(pPoint);   // 将地图主窗口的中心点定位到新的位置
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);    //主窗口地图刷新
            }
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            switch (toolAction)
            {
                case "Drag Zoom In"://判定当前的操作是否为拉框放大

                    IEnvelope pEnvelope = axMapControl1.TrackRectangle();   // 调用axMapControl1的矩形框跟踪功能进行拉框操作，
                    //将拉框完成后的范围赋值给一个Envelope对象
                    //此处，你可以尝试axMapControl1.TrackPolygon()，axMapControl1.TrackCircle()，axMapControl1.TrackLine()等方法的调用，看有什么效果
                    if (pEnvelope == null || pEnvelope.IsEmpty || pEnvelope.Height == 0 || pEnvelope.Width == 0)   //判定所获取的拉框范围是否无效
                    {
                        return;  // 拉框范围无效，就直接返回
                    }
                    axMapControl1.Extent = pEnvelope;  // 给地图窗口的空间范围赋值为刚才拉框的范围
                    axMapControl1.ActiveView.Refresh();  //地图刷新
                    return;
                case "Drag Zoom Out":   //判定当前的操作是否为拉框缩小
                    IEnvelope pEnvelope1 = axMapControl1.TrackRectangle();    // 调用axMapControl1的矩形框跟踪功能进行拉框操作，
                    //将拉框完成后的范围赋值给一个Envelope对象
                    //此处，你可以尝试axMapControl1.TrackPolygon()，axMapControl1.TrackCircle()，axMapControl1.TrackLine()等方法的调用，看有什么效果 
                    if (pEnvelope1 == null || pEnvelope1.IsEmpty || pEnvelope1.Height == 0 || pEnvelope1.Width == 0)   ////判定所获取的拉框范围是否无效
                    {
                        return;  //// 拉框范围无效，就直接返回
                    }
                    IActiveView pActiveView = axMapControl1.ActiveView;   // 获取地图视窗对象
                    double mHeight = pActiveView.Extent.Height * pActiveView.Extent.Height / pEnvelope1.Height;   // 根据倍数计算新空间范围高度
                    double mWidth = pActiveView.Extent.Width * pActiveView.Extent.Width / pEnvelope1.Width;   // 根据倍数计算新空间范围宽度
                    double mMinX = pActiveView.Extent.XMin - (pEnvelope1.XMin - pActiveView.Extent.XMin) * pActiveView.Extent.Width / pEnvelope1.Width;   //计算新空间范围的x的最小值
                    double mMinY = pActiveView.Extent.YMin - (pEnvelope1.YMin - pActiveView.Extent.YMin) * pActiveView.Extent.Height / pEnvelope1.Height;  //计算新空间范围的y的最小值
                    double mMaxX = mMinX + mWidth;  //计算新空间范围的x的最大值
                    double mMaxY = mMinY + mHeight;  // 计算新空间范围的y的最大值
                    pEnvelope1.PutCoords(mMinX, mMinY, mMaxX, mMaxY);  // 给Envelope对象赋值新的空间范围

                    //(pActiveView.Extent.XMax-pActiveView.Extent.XMin)*
                    axMapControl1.Extent = pEnvelope1;  // 给地图范围赋值为新的空间范围
                    axMapControl1.ActiveView.Refresh();  //地图刷新
                    return;
                case "Pan":    // 判定是否进行地图平移操作
                    axMapControl1.Pan();  //可执行地图平移
                    return;
                case "Select Feature Using Point":   //点选要素
                   
                    axMapControl1.ActiveView.FocusMap.ClearSelection();   // 清空当前地图的选择集;
                    axMapControl1.ActiveView.Refresh();
                    tagRECT r;   //   定义一个tagRECT结构变量，用于存放以当前鼠标为中心的长宽各为10个像素的小矩形
                    r.left = e.x - 5;    //
                    r.right = e.x + 5;   //
                    r.bottom = e.y - 5;   //
                    r.top = e.y + 5;     //
                    IEnvelope pEnv = new Envelope() as IEnvelope;   // 创建一个Envelope对象，用于接受刚才创建的小矩形范围由屏幕坐标转换为地图坐标的值
                    axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnv, ref r, 4);     //把r的屏幕坐标转换为地图坐标，存入Envelope对象中
                    pEnv.SpatialReference = axMapControl1.ActiveView.FocusMap.SpatialReference;    //设定Envelope对象的空间坐标参考与当前地图窗口的坐标一致
                    IGeometry pGeo = pEnv as IGeometry;     //接口访问到IGeometry接口
                    axMapControl1.Map.SelectByShape(pGeo, null, false);    //调用地图控件中map对象的SelectByShape方法，在地图中选择与pGeo相交或包含在其中的地图要素
                    //int selCount = axMapControl1.Map.SelectionCount;
                    //IEnumFeature pEnumFeature = axMapControl1.Map.FeatureSelection as IEnumFeature;
                    //IFeature pFeature = pEnumFeature.Next();
                    //while (pFeature != null)
                    //{
                    //    int index = pFeature.Fields.FindField("GB");
                    //    str1 = Convert.ToString(pFeature.get_Value(index));
                    //    MessageBox.Show(str1);
                    //    //string str2 = pFeature.get_Value(2).ToString();
                    //    pFeature = pEnumFeature.Next();
                    //}


                   
                    ISelection selection =axMapControl1.Map.FeatureSelection;
                    IEnumFeatureSetup iEnumFeatureSetup = (IEnumFeatureSetup)selection;
                    iEnumFeatureSetup.AllFields = true;
                    IEnumFeature pEnumFeature = (IEnumFeature)iEnumFeatureSetup;
                    pEnumFeature.Reset();
                    IFeature pFeature = pEnumFeature.Next();
                    if (pFeature != null)
                    {
                        int index = pFeature.Fields.FindField("RNAME");
                        //string str1 = pFeature.OID.ToString();
                        if(index!=-1)
                        str1 = pFeature.get_Value(index).ToString();
                        //MessageBox.Show(str1);
                        //pFeature = pEnumFeature.Next();
                    }
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);    //
                    
                    toolAction = "";
                    return;
                case "Select Feature Using Rectangle":     //矩形选择要素
                    IEnvelope pEnvlope2 = axMapControl1.TrackRectangle();     //  进行矩形框的绘制追踪，
                    if (pEnvlope2.IsEmpty == true)     // 若矩形框为空
                    {
                        tagRECT r1;
                        r1.left = e.x - 5;
                        r1.right = e.x + 5;
                        r1.bottom = e.y - 5;
                        r1.top = e.y + 5;
                        axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvlope2, ref r1, 4);
                        pEnvlope2.SpatialReference = axMapControl1.ActiveView.FocusMap.SpatialReference;
                    }
                    IGeometry pGeo1 = pEnvlope2 as IGeometry;
                    axMapControl1.Map.SelectByShape(pGeo1, null, false);
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                   
                    
                    toolAction = "";
                    return;


                case "Select Feature Using Circle":   // 圆形选择要素
                    IGeometry pCircl = axMapControl1.TrackCircle();   //圆形绘制
                    if (pCircl.IsEmpty == true)
                    {
                        return;
                    }
                    // IGeometry pGeo1 = pEnvlope2 as IGeometry;
                    axMapControl1.Map.SelectByShape(pCircl, null, false);
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    toolAction = "";
                    return;

                case "Select Feature Using Polygon":   //多边形选择要素

                    IGeometry pPolygon = axMapControl1.TrackPolygon();   // 绘制多边形
                    if (pPolygon.IsEmpty == true)
                    {
                        return;
                    }
                    // IGeometry pGeo1 = pEnvlope2 as IGeometry;
                    axMapControl1.Map.SelectByShape(pPolygon, null, false);
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    toolAction = "";
                    return;
                case "Select Feature Using Polyline":    // 线性选择要素

                    IGeometry pLine = axMapControl1.TrackLine();   //绘制线
                    if (pLine.IsEmpty == true)
                    {
                        return;
                    }
                    // IGeometry pGeo1 = pEnvlope2 as IGeometry;
                    axMapControl1.Map.SelectByShape(pLine, null, false);
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    toolAction = "";
                    return;
                case "Draw Polygon":
                    IGeometry pPolygon1 = axMapControl1.TrackPolygon();   // 绘制多边形
                    if (pPolygon1 == null)
                    {
                        return;
                    }
                    IElement pElement = new PolygonElement();
                    pElement.Geometry = pPolygon1;
                    IFillShapeElement pPolygonEle = pElement as IFillShapeElement;
                    if (pSelectedFillSymbol != null)
                        pPolygonEle.Symbol = pSelectedFillSymbol;

                    IGraphicsContainer pGc = axMapControl1.ActiveView.GraphicsContainer;
                    pGc.AddElement(pElement, 0);
                    IGraphicsContainerSelect pGSelection = pGc as IGraphicsContainerSelect;
                    pGSelection.UnselectAllElements();
                    pGSelection.SelectElement(pElement);

                    toolAction = "";
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    return;
                case "Draw Point":
                    IPoint pPointMark = new ESRI.ArcGIS.Geometry.Point();   // 创建一个点

                    pPointMark.PutCoords(e.mapX, e.mapY);   //给点赋值为当前鼠标所在位置的地图坐标
                    IGeometry pPointGeo = pPointMark as IGeometry;
                    IElement pMElement = new MarkerElement();
                    pMElement.Geometry = pPointGeo;
                    IMarkerElement pMarkElement = pMElement as IMarkerElement;
                    if (pSelectedMarkSymbol != null)
                        pMarkElement.Symbol = pSelectedMarkSymbol;

                    IGraphicsContainer pGraphiscont = axMapControl1.ActiveView.GraphicsContainer;
                    pGraphiscont.AddElement(pMElement, 0);
                    IGraphicsContainerSelect pGS = pGraphiscont as IGraphicsContainerSelect;
                    pGS.UnselectAllElements();
                    pGS.SelectElement(pMElement);

                    toolAction = "";
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    return;
                case "Draw Line":
                    IGeometry pLineGeo = axMapControl1.TrackLine();   // 绘制Line
                    if (pLineGeo == null)
                    {
                        return;
                    }
                    IElement pElement4 = new LineElement();
                    ILineElement pLineEle = pElement4 as ILineElement;
                    if (pSelectedLineSymbol != null)
                        pLineEle.Symbol = pSelectedLineSymbol;

                    pElement4.Geometry = pLineGeo;
                    IGraphicsContainer pGc4 = axMapControl1.ActiveView.GraphicsContainer;
                    pGc4.AddElement(pElement4, 0);
                    IGraphicsContainerSelect pGSelection4 = pGc4 as IGraphicsContainerSelect;
                    pGSelection4.UnselectAllElements();
                    pGSelection4.SelectElement(pElement4);

                    toolAction = "";
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    return;

                case "Draw Text":
                    IRgbColor pColor = new RgbColor();
                    pColor.Red = 150;
                    pColor.Green = 150;
                    pColor.Blue = 0;

                    ITextSymbol pTextSymbol = new TextSymbol();
                    stdole.StdFont font = new stdole.StdFont();
                    font.Name = "宋体";
                    font.Size = 20;
                    pTextSymbol.Font = font as stdole.IFontDisp;
                    pTextSymbol.Color = pColor;
                    pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                    pTextSymbol.Text = "This is my First Text";


                    IPoint pTextPoint = new ESRI.ArcGIS.Geometry.Point();   // 创建一个点

                    pTextPoint.PutCoords(e.mapX, e.mapY);   //给点赋值为当前鼠标所在位置的地图坐标
                    IGeometry pTextGeo = pTextPoint as IGeometry;

                    IElement pTElement = new TextElement();
                    pTElement.Geometry = pTextGeo;
                    ITextElement pTextEle = pTElement as ITextElement;
                    pTextEle.Symbol = pTextSymbol;
                    pTextEle.Text = pTextSymbol.Text;

                    IGraphicsContainer pGraphisCon = axMapControl1.ActiveView.GraphicsContainer;
                    pGraphisCon.AddElement(pTElement, 0);

                    IGraphicsContainerSelect pGrhSelection = pGraphisCon as IGraphicsContainerSelect;
                    pGrhSelection.UnselectAllElements();
                    pGrhSelection.SelectElement(pTElement);

                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    toolAction = "";
                    break;


                case "Select Graphics by using Point":    //通过点击获得GraphicsContainor中的element

                    IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();   // 创建一个点
                    pPoint.PutCoords(e.mapX, e.mapY);   //给点赋值为当前鼠标所在位置的地图坐标
                    double dist = axMapControl1.ActiveView.Extent.Width / 15;   //设置以点为圆心的查询的半径
                    IGraphicsContainer pGc1 = axMapControl1.ActiveView.GraphicsContainer;   //获得地图的GraphicsContainor容器
                    IEnumElement pEnumElement = pGc1.LocateElements(pPoint, dist);   //以点为中心点，按照指定半径选择element
                    IGraphicsContainerSelect pGSelection1 = pGc1 as IGraphicsContainerSelect;   //获得地图的容器中的图形要素选择集
                    if (pEnumElement != null)   //
                    {
                        pEnumElement.Reset();   //回到-1位置
                        pGSelection1.UnselectAllElements();   //清空原来选择到的图形要素
                        pGSelection1.SelectElements(pEnumElement);   //将新查询到的图形要素添加到GraphicsContainor容器中的图形要素选择集中
                    }
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);   //图形要素刷新

                    return;
                    


            }
            if (e.button == 2)   //若单击鼠标右键
            {
                contextMenuStrip2.Show(axMapControl1, e.x, e.y);    //在TOC控件的当前鼠标位置弹出右键菜单
            }

        }

        private void barButtonItem28_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {         
            toolAction = "Select Feature Using Rectangle";
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            toolAction = "Select Feature Using Point";
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            toolAction = "Select Feature Using Polygon";
        }

        private void barButtonItem29_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            toolAction = "";
            axMapControl1.ActiveView.FocusMap.ClearSelection();   // 清空当前地图的选择集;
            axMapControl1.ActiveView.Refresh();
        }

        private void barButtonItem30_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IMap pMap = axMapControl1.ActiveView.FocusMap;
            StaticsForm1 frmStatistics = new StaticsForm1();  //创建属性统计窗口
            //frmStatistics.PFeatureClass =pGlobeFeatureClass;
            frmStatistics.PMap = pMap;
            //frmStatistics.PFeatureClass = pGlobeFeatureLayer.FeatureClass;    //给窗口属性赋值
            frmStatistics.Show();     //打开窗口
        }

        private void barButtonItem27_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IMap pMap = axMapControl1.ActiveView.FocusMap;   //获取当前地图
            AttributeQueryFormcs frmAtrributeQuery = new AttributeQueryFormcs();   //创建属性查询窗口
            frmAtrributeQuery.PMap = pMap;   //给属性查询窗口的PMap属性赋值
            frmAtrributeQuery.Show();  //打开窗口
        }

        private void 图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolAction = "Select Feature Using Point";
            if (str1 != null)
            {
                PictureShowForm pic = new PictureShowForm(str1);
                pic.Show();
            }
            str1 = "";
            toolAction = "";




        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem31_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pZoomIn = new ControlsMapZoomInTool();
            pZoomIn.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomIn as ITool;
        }

        private void barButtonItem32_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pZoomOut = new ControlsMapZoomOutTool();
            pZoomOut.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomOut as ITool;
        }

        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.ActiveView.Extent = axMapControl1.ActiveView.FullExtent;
            axMapControl1.ActiveView.Refresh();
        }

        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IExtentStack pExtentStack = null;

            pExtentStack = axMapControl1.ActiveView.ExtentStack;

            if (pExtentStack.CanUndo())

            {

                pExtentStack.Undo();

            }

            axMapControl1.Refresh();
        }

        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IExtentStack pExtentStack = null;

            pExtentStack = axMapControl1.ActiveView.ExtentStack;

            if (pExtentStack.CanRedo())

            {

                pExtentStack.Redo();

            }

            axMapControl1.Refresh();
        }

        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pZoomPan = new ControlsMapZoomPanTool();
            pZoomPan.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomPan as ITool;
        }

        private void barButtonItem37_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.CurrentTool = null;
        }
    }
}
