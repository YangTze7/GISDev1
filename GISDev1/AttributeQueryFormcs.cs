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
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;

namespace GISDev1
{
    public partial class AttributeQueryFormcs : DevExpress.XtraEditors.XtraForm
    {

        private IMap _pMap = null; //存放地图对象
        private IField _pSelectedField = null;   //存放当前的选择的字段
        private IFeatureLayer _pSelectedFeatureLayer = null;   //存放当前选择的图层；

        private IFeatureClass _pSelectedFeatureClass = null;   //存放当前选择的要素类
        //private string _sCurrentWhere = "";   //存放当前的设置的where条件
        //private string _sSelectedLayerName = ""; //存放当前层的名称
        private string _sSelectedFieldName = "";  //当前选择的字段名称
        private string _sWhereFirstPart = ""; //条件文本框里面光标所在位置的前半部分字符串内容
        private string _sWhereSecondPart = ""; //条件文本框里面光标所在位置的后半部分字符串内容
        private int _iCursorPosion = 0; //条件文本框里面光标所在的位置

        public IMap PMap
        {
            get { return _pMap; }
            set { _pMap = value; }
        }

        public AttributeQueryFormcs()
        {
            InitializeComponent();
        }

        private void AttributeQueryFormcs_Load(object sender, EventArgs e)
        {
            if (PMap.LayerCount > 0)  //若地图中的图层不为空
            {
                comLayerName.Items.Clear();   //清除图层名称列表框中的原有内容
                IEnumLayer pEnumlayer = PMap.Layers;   //获取地图中的图层集合
                pEnumlayer.Reset();   //
                ILayer pLayer = pEnumlayer.Next();    //获得第一个图层
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer; //获得矢量要素层
                while (pFeatureLayer != null)   //
                {
                    string name = pLayer.Name;   //获得图层名称
                    comLayerName.Items.Add(name);   //将图层名称添加到列表中
                    pLayer = pEnumlayer.Next();    //获得下一个图层
                    pFeatureLayer = pLayer as IFeatureLayer; //获得矢量要素层
                }
            }

        }
        private void listFields_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listFields.SelectedItem != null)
            {
                string lsSeletedName = listFields.SelectedItem.ToString(); //获取选择到的字段名称
                if (lsSeletedName != _sSelectedFieldName)
                    listUniqueValue.Enabled = false;   //改变唯一值列表框中的选择状态为不可选
                _sSelectedFieldName = lsSeletedName; //保存起来选择的字段名称
                _sWhereFirstPart = _sWhereFirstPart + " " + _sSelectedFieldName + " "; //将字段名称追加到条件文本串第一部分的后面
                // txtWhere.SelectionStart = _sWhereFirstPart.Length;   
                _iCursorPosion = _sWhereFirstPart.Length;   //获得当前光标的位置
                textBox1.Select(_sWhereFirstPart.Length, 0);// 将文本框的光标位置设置到第一部分文本串的最后
                textBox1.Text = _sWhereFirstPart + _sWhereSecondPart;   // 给文本框赋值为前后两部分合并后的字符串
                //  txtWhere.Focus();   //让文本框聚焦

                IFields pFields = _pSelectedFeatureClass.Fields;   //获得当前选择的要素类的字段集合
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    IField pField = pFields.get_Field(i);   //获得第i个字段
                    string lsname = pField.Name;   //获得字段名称
                                                   // lsname = "\"" + lsname + "\"";
                    if (lsname == _sSelectedFieldName)   //若字段名称与选择字段名称相同
                    {
                        _pSelectedField = pField; //获得当前字段赋值给类的私有变量
                        return;
                    }
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
                    return "System.Int32";
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


        private void button1_Click(object sender, EventArgs e)
        {
            if (_pSelectedField != null)
            {
                listUniqueValue.Enabled = true;
                listUniqueValue.Items.Clear();

                IQueryFilter queryFilter = new QueryFilterClass(); // 创建查询过滤条件对象
                queryFilter.SubFields = _pSelectedField.Name;   // 设置返回的字段名称

                IFeatureCursor pFeatureCursor = _pSelectedFeatureClass.Search(queryFilter, true); //得到IFeatureCursor游标
                ICursor pCursor = pFeatureCursor as ICursor;  //接口查询返回ICursor游标
                IDataStatistics pDataStatistics = new DataStatistics();   //创建DataStatistics对象，该对象用于进行字段值的统计以及获取字段的唯一值
                pDataStatistics.Cursor = pCursor;    //给DataStatistics对象的游标属性赋值
                pDataStatistics.Field = _pSelectedField.Name;   //给DataStatistics对象的字段属性赋值

                IEnumerator pEnumerator = pDataStatistics.UniqueValues;   //获得唯一值数据集
                pEnumerator.Reset();   //
                while (pEnumerator.MoveNext())   //未到最后一条记录
                {
                    if (pEnumerator.Current != null)
                    {
                        string lsname = pEnumerator.Current.ToString();   //把当前值转成字符串
                        switch (ParseFieldType(_pSelectedField.Type))   //判断字段类型
                        {
                            case "System.String":   //若为字符串，
                                listUniqueValue.Items.Add("'" + lsname + "'");   //在文本字符串两端加上单引号，然后添加进唯一值列表控件中
                                break;
                            default:
                                listUniqueValue.Items.Add(lsname);   //若为其他类型的字段，则直接添加进唯一值列表控件中
                                break;
                        }
                    }

                }
            }
        }

        private void listUniqueValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listUniqueValue.SelectedItem != null)
            {
                string lsSeletedValue = listUniqueValue.SelectedItem.ToString(); //获取选择到的字段值
                _sWhereFirstPart = _sWhereFirstPart + " " + lsSeletedValue + " "; //将字段值追加到条件文本串第一部分的后面
                _iCursorPosion = _sWhereFirstPart.Length;   //获得当前光标的位置
                textBox1.Select(_sWhereFirstPart.Length, 0);// 将文本框的光标位置设置到第一部分文本串的最后
                textBox1.Text = _sWhereFirstPart + _sWhereSecondPart;   // 给文本框赋值为前后两部分合并后的字符串
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            _iCursorPosion =textBox1.SelectionStart; //获得光标起始位置
            string text = textBox1.Text;   //获得整个文本串

            _sWhereFirstPart = text.Substring(0, _iCursorPosion);  // 获得光标之前的文本
            _sWhereSecondPart = text.Substring(_iCursorPosion);  //获得光标之后的文本
            textBox1.Select(_iCursorPosion, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IQueryFilter pQueryFilter = new QueryFilter();   //定义属性查询过滤器
            pQueryFilter.WhereClause = textBox1.Text;  //获得属性查询条件
            IFeatureCursor pFeatureCursor = _pSelectedFeatureClass.Search(pQueryFilter, false);  // 根据属性查询条件在特征要素层中进行查询
            IFeature pFeature;
            IActiveView pActiveView = PMap as IActiveView;  // 获得ActiveView
            // ISelection pSelection = pActiveView.Selection;  //
            IFeatureSelection pFeatureSelection = _pSelectedFeatureLayer as IFeatureSelection;  //获得当前特征要素层的特征选择集
            //if (comSelectStyle.Text == "创建新选择集")  //  若为“创建新选择集”，则要清空原有选择集，否则向原选择集中添加新选择要素
            //    pFeatureSelection.Clear();  //清空选择集

            while ((pFeature = pFeatureCursor.NextFeature()) != null)  //依次获得查询到的特征要素
            {
                pFeatureSelection.Add(pFeature);                  //将特征要素添加到本层的选择集中
            }
            pActiveView.Refresh();  //地图刷新
        }

        private void listFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sSelectedFieldName = listFields.SelectedItem.ToString();  //获取选择到的字段名称
            listUniqueValue.Enabled = false;   //改变唯一值列表框中的选择状态为不可选
            IFields pFields = _pSelectedFeatureClass.Fields;   //获得当前选择的要素类的字段集合
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                IField pField = pFields.get_Field(i);   //获得第i个字段
                string lsname = pField.Name;   //获得字段名称
                                               // lsname = "\"" + lsname + "\"";
                if (lsname == _sSelectedFieldName)   //若字段名称与选择字段名称相同
                {
                    _pSelectedField = pField; //获得当前字段赋值给类的私有变量
                    return;
                }
            }
        }

        private void listFields_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (listFields.SelectedItem != null)
            {
                string lsSeletedName = listFields.SelectedItem.ToString(); //获取选择到的字段名称
                if (lsSeletedName != _sSelectedFieldName)
                    listUniqueValue.Enabled = false;   //改变唯一值列表框中的选择状态为不可选
                _sSelectedFieldName = lsSeletedName; //保存起来选择的字段名称
                _sWhereFirstPart = _sWhereFirstPart + " " + _sSelectedFieldName + " "; //将字段名称追加到条件文本串第一部分的后面
                // txtWhere.SelectionStart = _sWhereFirstPart.Length;   
                _iCursorPosion = _sWhereFirstPart.Length;   //获得当前光标的位置
                textBox1.Select(_sWhereFirstPart.Length, 0);// 将文本框的光标位置设置到第一部分文本串的最后
                textBox1.Text = _sWhereFirstPart + _sWhereSecondPart;   // 给文本框赋值为前后两部分合并后的字符串
                //  txtWhere.Focus();   //让文本框聚焦

                IFields pFields = _pSelectedFeatureClass.Fields;   //获得当前选择的要素类的字段集合
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    IField pField = pFields.get_Field(i);   //获得第i个字段
                    string lsname = pField.Name;   //获得字段名称
                                                   // lsname = "\"" + lsname + "\"";
                    if (lsname == _sSelectedFieldName)   //若字段名称与选择字段名称相同
                    {
                        _pSelectedField = pField; //获得当前字段赋值给类的私有变量
                        return;
                    }
                }

            }
        }

        private void comLayerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectLayerName = comLayerName.SelectedItem.ToString(); //获得当前选择到的图层名称
            IEnumLayer pEnumlayer = PMap.Layers;   //获取地图中的图层集合
            pEnumlayer.Reset();   //
            ILayer pLayer = pEnumlayer.Next();    //获得第一个图层
            while (pLayer != null)   //
            {
                string name = pLayer.Name;   //获得图层名称
                if (selectLayerName == name)  //在图层集合中找到相应名称的图层
                {
                    label1.Text = "SELECT * FROM " + name + " WHERE:"; //设置显示的选择数据条件的文本字符串
                    _pSelectedFeatureLayer = pLayer as IFeatureLayer;  //接口查询，获得矢量要素层
                    _pSelectedFeatureClass = _pSelectedFeatureLayer.FeatureClass;  //获得矢量层对应的数据源 特征类
                    IFields pFields = _pSelectedFeatureClass.Fields;   //获得要素类的字段集合
                    listFields.Items.Clear();  // 清除掉窗口中listview控件中显示的字段列表
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        IField pField = pFields.get_Field(i);   //获得第i个字段
                        string lsname = pField.Name;   //获得字段名称
                        if (lsname != _pSelectedFeatureClass.ShapeFieldName)   //若字段不是几何字段
                        {
                            // lsname = "\"" + lsname + "\"";
                            listFields.Items.Add(lsname);   // 将字段名称添加到listview字段列表控件中
                        }
                    }

                    return;
                }
                pLayer = pEnumlayer.Next();    //获得下一个图层
            }

        }
    }
}