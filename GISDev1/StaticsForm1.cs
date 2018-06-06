using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

namespace GISDev1
{
    public partial class StaticsForm1 : DevExpress.XtraEditors.XtraForm
    {
        private IMap _pMap = null; //存放地图对象
        private IFeatureClass _pFeaturClass;
        private ILayer pLayer = null;   //存放当前选择的图层；
        

        public IFeatureClass PFeatureClass
        {
            get { return _pFeaturClass; }
            set { _pFeaturClass = value; }
        }

        public IMap PMap
        {
            get { return _pMap; }
            set { _pMap = value; }
        }
        public StaticsForm1()
        {
            InitializeComponent();
        }

        private void StaticsForm1_Load(object sender, EventArgs e)
        {
           

            if (PMap.LayerCount > 0)  //若地图中的图层不为空
            {
                comboBox2.Items.Clear();   //清除图层名称列表框中的原有内容
                IEnumLayer pEnumlayer = PMap.Layers;   //获取地图中的图层集合
                pEnumlayer.Reset();   //
               pLayer = pEnumlayer.Next();    //获得第一个图层
                //_pSelectedFeatureLayer = pLayer1 as IFeatureLayer; //获得矢量要素层
                while (pLayer != null)   //
                {
                    string name = pLayer.Name;   //获得图层名称
                    comboBox2.Items.Add(name);   //将图层名称添加到列表中
                    pLayer = pEnumlayer.Next();    //获得下一个图层

                }

            }
         
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string fieldName = comboBox1.SelectedItem.ToString();
            string selectLayerName = comboBox2.SelectedItem.ToString();
            IEnumLayer pEnumlayer = PMap.Layers;   //获取地图中的图层集合
            pEnumlayer.Reset();   //
            ILayer pLayer = pEnumlayer.Next();    //获得第一个图层


            while (pLayer != null)   //
            {
                string name = pLayer.Name;   //获得图层名称
                if (selectLayerName == name)  //在图层集合中找到相应名称的图层
                {
                    IFeatureLayer featureLayer = pLayer as IFeatureLayer;
                  
                    if (fieldName != null)
                    {
                        IFields pFields = featureLayer.FeatureClass.Fields;   //获得特征类的字段集合
                        for (int i = 0; i < pFields.FieldCount; i++)    // 循环
                        {
                            IField pField = pFields.get_Field(i);    //获得第i个字段
                            if (pField.Name == fieldName)
                            {

                                IQueryFilter queryFilter = new QueryFilterClass(); // 创建查询过滤条件对象
                                queryFilter.SubFields = fieldName;   // 设置返回的字段名称

                                IFeatureCursor pFeatureCursor = featureLayer.FeatureClass.Search(queryFilter, true); //得到IFeatureCursor游标
                                ICursor pCursor = pFeatureCursor as ICursor;  //接口查询返回ICursor游标
                                IDataStatistics pDataStatistics = new DataStatistics();   //创建DataStatistics对象，该对象用于进行字段值的统计
                                pDataStatistics.Cursor = pCursor;    //给DataStatistics对象的游标属性赋值
                                pDataStatistics.Field = fieldName;   //给DataStatistics对象的字段属性赋值

                                IStatisticsResults pStatisticsResult = pDataStatistics.Statistics;   //获得统计结果
                                label2.Text = pStatisticsResult.Maximum.ToString();    //最大值
                                label3.Text = pStatisticsResult.Minimum.ToString();    //最小值
                                label4.Text = pStatisticsResult.Mean.ToString();     //平均值
                                label5.Text = pStatisticsResult.StandardDeviation.ToString();   //标准差
                                label6.Text = pStatisticsResult.Count.ToString();   //个数
                                label7.Text = pStatisticsResult.Sum.ToString();   //总和


                                return;
                            }
                        }
                    }






                    return;
                }
                pLayer = pEnumlayer.Next();    //获得下一个图层
            }








            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string selectLayerName = comboBox2.SelectedItem.ToString();
            IEnumLayer pEnumlayer = PMap.Layers;   //获取地图中的图层集合
            pEnumlayer.Reset();   //
            ILayer pLayer = pEnumlayer.Next();    //获得第一个图层


            while (pLayer != null)   //
            {
                string name = pLayer.Name;   //获得图层名称
                if (selectLayerName == name)  //在图层集合中找到相应名称的图层
                {
                    IFeatureLayer featureLayer = pLayer as IFeatureLayer;
                    IFields pFields = featureLayer.FeatureClass.Fields;   //获得特征类的字段集合
                    for (int i = 0; i < pFields.FieldCount; i++)    // 循环
                    {
                        IField pField = pFields.get_Field(i);    //获得第i个字段
                        switch (pField.Type)     //判断字段的类型，若是数值型的就把其名称添加到字段列表中
                        {
                            case esriFieldType.esriFieldTypeDouble:    //
                            case esriFieldType.esriFieldTypeInteger:    //
                            case esriFieldType.esriFieldTypeSingle:     //
                            case esriFieldType.esriFieldTypeSmallInteger:    //
                                comboBox1.Items.Add(pField.Name);     //
                                break;
                        }
                    }
                    return;
                }
                pLayer = pEnumlayer.Next();    //获得下一个图层
            }



        }

    
    }
}
