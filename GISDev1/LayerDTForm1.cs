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
using ESRI.ArcGIS.Carto;

namespace GISDev1
{
    public partial class LayerDTForm1 : DevExpress.XtraEditors.XtraForm
    {
      private IFeatureLayer _pFeatureLayer;

      public IFeatureLayer PFeatureLayer   //属性，用于存放外界赋值的特征要素层
      {
          get { return _pFeatureLayer; }
          set { _pFeatureLayer = value; }
      }
        public LayerDTForm1()
        {
            InitializeComponent();
        }
        private void LayerDTForm1_Load(object sender,EventArgs e)
        {
          if (PFeatureLayer != null)
          {
              IFeatureClass pFeatureClass = PFeatureLayer.FeatureClass;  //获得要素层所对应的要素类 数据源
              IFields pFields = pFeatureClass.Fields;  // 获得要素类的字段集合对象
              dataGridView1.ColumnCount = pFields.FieldCount;   // 将字段的个数给DataGridView控件的列数赋值
              for (int i = 0; i < pFields.FieldCount; i++)   //
              {
                  IField pField=pFields.get_Field(i);   // 从字段集合中获得第i个字段对象
                  dataGridView1.Columns[i].Name = pField.Name;    //给DataGridView的列附值为获得的字段名字
                  dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pField.Type));         //    给DataGridView列的数据类型赋值为字段数据类型转换后的类型，具体的转换由ParseFieldType函数实现；
              }
              IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);    // 对要素类进行无条件查询，查询的结果为全部的要素所构成的要素集合对象，并返回要素游标接口
              IFeature pFeature = pFeatureCursor.NextFeature();    //获得要素集合中的第一条要素
              while (pFeature != null)   //若要素非空
              {
                  string[] fieldsValue = new string[pFields.FieldCount];   // 创建一个字符串数组，用于存放当前要素所有字段的内容
                  for (int i = 0; i < pFields.FieldCount ; i++)
                  {
                      string fieldname = pFields.get_Field(i).Name;    // 获得当前字段名称
                        if (fieldname == pFeatureClass.ShapeFieldName)   // 若当前字段是要素类的存放几何数据的字段
                            fieldsValue[i] = Convert.ToString(pFeature.Shape.GeometryType).Substring(12);   // 则获取要素的几何类型名称
                        else
                            fieldsValue[i] = Convert.ToString(pFeature.get_Value(i));   //否则就获取当前字段的属性值
                  }
                  dataGridView1.Rows.Add(fieldsValue);    // 将字符串数组添加到DataGridView控件的行集中

                  pFeature = pFeatureCursor.NextFeature();   //获得要素集合中的下一条要素，若到了最后一个要素，就返回空值
              }
          }
        }
        public string ParseFieldType(esriFieldType fieldType)
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";

                default:
                    return "System.String";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
