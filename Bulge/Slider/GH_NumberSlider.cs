// Decompiled with JetBrains decompiler
// Type: Grasshopper.Kernel.Special.GH_NumberSlider
// Assembly: Grasshopper, Version=7.33.23248.13001, Culture=neutral, PublicKeyToken=dda4f5ec2cd80803
// MVID: 5BB20879-33FF-4DFC-95F1-EEC62DAC6ECD
// Assembly location: C:\Users\GV\Desktop\Grasshopper\Grasshopper.dll
// XML documentation location: C:\Users\GV\Desktop\Grasshopper\Grasshopper.xml

using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Base;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.HTML;
using Grasshopper.GUI.RemotePanel;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Expressions;
using Grasshopper.Kernel.Types;
using Grasshopper.My.Resources;
using Microsoft.VisualBasic.CompilerServices;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bulge.Slider
{
    /// <exclude />
    public class GH_NumberSlider :
      GH_Param<GH_Number>,
      IGH_StateAwareObject
    {
        private readonly GH_SliderBase m_slider;
        private Decimal m_cache;
        private string m_expression;
        private readonly List<string> _snapRanges;

        public GH_NumberSlider()
          : base((IGH_InstanceDescription)new GH_InstanceDescription("Number Slider", string.Empty, "Numeric slider for single values", "Params", "Input"))
        {
            this.m_slider = new GH_SliderBase();
            this.m_cache = Decimal.MinValue;
            this.m_expression = (string)null;
            this._snapRanges = new List<string>();
            this.m_slider.Type = GH_SliderAccuracy.Float;
            this.m_slider.Minimum = 0M;
            this.m_slider.Maximum = 1M;
            this.m_slider.Value = 0.25M;
            this.m_slider.DecimalPlaces = 3;
            this.m_slider.GripDisplay = GH_SliderGripDisplay.ShapeAndText;
            this.m_slider.Font = GH_FontServer.StandardAdjusted;
            this.ResetCurrentValue();
            this.m_slider.ValueChanged += new GH_SliderBase.ValueChangedEventHandler(this.InternalSliderValueChanged);
        }

        public void SetInitCode(string code)
        {
            try
            {
                code = code.Trim();
                GH_Variant expression1 = GH_Convert.ParseExpression(code, true);
                if (expression1.Type != GH_VariantType.@null)
                {
                    switch (expression1.Type)
                    {
                        case GH_VariantType.@int:
                            int val1_1 = expression1._Int;
                            this.Slider.Type = GH_SliderAccuracy.Integer;
                            if (val1_1 == 0)
                            {
                                this.Slider.Minimum = 0M;
                                this.Slider.Maximum = 100M;
                                this.Slider.Value = 0M;
                                goto label_21;
                            }
                            else if (val1_1 < 0)
                            {
                                int num = Math.Max(val1_1, -100000000);
                                this.Slider.Minimum = Convert.ToDecimal(GH_Convert.ToPrevPowerOfTen((double)num));
                                this.Slider.Maximum = 0M;
                                this.Slider.Value = new Decimal(num);
                                goto label_21;
                            }
                            else
                            {
                                int num = Math.Min(val1_1, 100000000);
                                this.Slider.Minimum = 0M;
                                this.Slider.Maximum = Convert.ToDecimal(GH_Convert.ToNextPowerOfTen((double)num));
                                this.Slider.Value = new Decimal(num);
                                goto label_21;
                            }
                        case GH_VariantType.@double:
                            double val1_2 = expression1._Double;
                            this.Slider.Type = GH_SliderAccuracy.Float;
                            int num1 = GH_NumberSlider.HarvestDecimalPlaces(code);
                            if (num1 == 0)
                                num1 = 3;
                            this.Slider.DecimalPlaces = num1;
                            if (val1_2 == 0.0)
                            {
                                this.Slider.Minimum = 0M;
                                this.Slider.Maximum = 1M;
                                this.Slider.Value = 0M;
                                goto label_21;
                            }
                            else if (val1_2 < 0.0)
                            {
                                double num2 = Math.Max(val1_2, -100000000.0);
                                this.Slider.Minimum = Convert.ToDecimal(GH_Convert.ToPrevPowerOfTen(num2));
                                this.Slider.Maximum = 0M;
                                this.Slider.Value = Convert.ToDecimal(num2);
                                goto label_21;
                            }
                            else
                            {
                                double num3 = Math.Min(val1_2, 100000000.0);
                                this.Slider.Minimum = 0M;
                                this.Slider.Maximum = Convert.ToDecimal(GH_Convert.ToNextPowerOfTen(num3));
                                this.Slider.Value = Convert.ToDecimal(num3);
                                goto label_21;
                            }
                    }
                }
                string minimum = (string)null;
                string maximum = (string)null;
                string str = (string)null;
                if (GH_NumberSlider.HarvestRange(code, out minimum, out maximum, out str))
                {
                    GH_Variant expression2 = GH_Convert.ParseExpression(minimum, true);
                    GH_Variant expression3 = GH_Convert.ParseExpression(maximum, true);
                    GH_Variant expression4 = GH_Convert.ParseExpression(str, true);
                    if (expression2.Type == GH_VariantType.@int && expression3.Type == GH_VariantType.@int && expression4.Type == GH_VariantType.@int)
                    {
                        this.Slider.Type = GH_SliderAccuracy.Integer;
                        this.Slider.DecimalPlaces = 3;
                        this.Slider.Minimum = new Decimal(Math.Min(expression2._Int, expression3._Int));
                        this.Slider.Maximum = new Decimal(Math.Max(expression2._Int, expression3._Int));
                        this.Slider.Value = new Decimal(expression4._Int);
                    }
                    else
                    {
                        int num4 = Math.Max(Math.Max(Math.Max(0, GH_NumberSlider.HarvestDecimalPlaces(minimum)), GH_NumberSlider.HarvestDecimalPlaces(maximum)), GH_NumberSlider.HarvestDecimalPlaces(str));
                        if (num4 == 0)
                            num4 = 3;
                        this.Slider.Type = GH_SliderAccuracy.Float;
                        this.Slider.DecimalPlaces = num4;
                        this.Slider.Minimum = Convert.ToDecimal(Math.Min(expression2._Double, expression3._Double));
                        this.Slider.Maximum = Convert.ToDecimal(Math.Max(expression2._Double, expression3._Double));
                        this.Slider.Value = Convert.ToDecimal(expression4._Double);
                    }
                }
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
        label_21:
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.ResetCurrentValue();
        }

        /// <summary>
        /// Try and harvest the number of decimal places implied by a piece of text.
        /// Text must represent a regular number.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>The number of decimal places (though never more than 16) or 0 if no decimal places could be harvested.</returns>
        public static int HarvestDecimalPlaces(string text)
        {
            text = text.Trim();
            int num1;
            if (text.Length == 0)
            {
                num1 = 0;
            }
            else
            {
                bool flag = false;
                int val1 = 0;
                int num2 = text.Length - 1;
                for (int index = 0; index <= num2; ++index)
                {
                    char c = text[index];
                    if (index != 0 || c != '-' && c != '+')
                    {
                        if (c == '.')
                        {
                            if (flag)
                            {
                                num1 = 0;
                                goto label_15;
                            }
                            else
                                flag = true;
                        }
                        else if (!char.IsDigit(c))
                        {
                            num1 = 0;
                            goto label_15;
                        }
                        else if (flag)
                            ++val1;
                    }
                }
                num1 = Math.Min(val1, 16);
            }
        label_15:
            return num1;
        }

        /// <summary>
        /// Parse an init code for validity. Valid init codes are A&lt;B, A&lt;B&lt;C, A..B, A..B..C
        /// Where A, B and C can all be evaluated to doubles or integers. This method does not validate whether the
        /// actual values of A, B and C are non-decremental.
        /// Note that sliders can also be initiated using init codes that evaluate to single numbers. This method
        /// does not take those into account. This is purely for validating range inits.
        /// </summary>
        /// <param name="text">Init code to parse.</param>
        /// <param name="minimum">If text is a valid init code, the minimum descriptor will be returned here.</param>
        /// <param name="maximum">If text is a valid init code, the maximum descriptor will be returned here.</param>
        /// <param name="value">If text is a valid init code with a value portion, the value descriptor will be returned here.</param>
        /// <returns>True if the text is a valid init code.</returns>
        public static bool HarvestRange(
          string text,
          out string minimum,
          out string maximum,
          out string value)
        {
            minimum = string.Empty;
            maximum = string.Empty;
            value = string.Empty;
            bool flag1;
            if (string.IsNullOrWhiteSpace(text))
            {
                flag1 = false;
            }
            else
            {
                string[] strArray = new string[3]
                {
          string.Empty,
          string.Empty,
          string.Empty
                };
                int index1 = 0;
                GH_NumberSlider.InitNotation initNotation = GH_NumberSlider.InitNotation.None;
                bool flag2 = false;
                int num1 = 0;
                int num2 = text.Length - 1;
                for (int index2 = 0; index2 <= num2; ++index2)
                {
                    char ch = text[index2];
                    if (ch == '"')
                    {
                        //flag2 = !flag2;
                        //// ISSUE: variable of a reference type
                        //string&local;
                        //// ISSUE: explicit reference operation
                        //string str = ^(local = ref strArray[index1]) + Conversions.ToString(ch);
                        //local = str;
                    }
                    else if (flag2)
                    {
                        //// ISSUE: variable of a reference type
                        //string&local;
                        //// ISSUE: explicit reference operation
                        //string str = ^(local = ref strArray[index1]) + Conversions.ToString(ch);
                        //local = str;
                    }
                    else
                    {
                        switch (ch)
                        {
                            case '(':
                            case '[':
                            case '{':
                                //++num1;
                                //// ISSUE: variable of a reference type
                                //string&local1;
                                //// ISSUE: explicit reference operation
                                //string str1 = ^(local1 = ref strArray[index1]) + Conversions.ToString(ch);
                                //local1 = str1;
                                continue;
                            case ')':
                            case ']':
                            case '}':
                                --num1;
                                if (num1 < 0)
                                {
                                    flag1 = false;
                                    goto label_51;
                                }
                                else
                                {
                                    //// ISSUE: variable of a reference type
                                    //string&local2;
                                    //// ISSUE: explicit reference operation
                                    //string str2 = ^(local2 = ref strArray[index1]) + Conversions.ToString(ch);
                                    //local2 = str2;
                                    continue;
                                }
                            default:
                                if (num1 > 0)
                                {
                                    //// ISSUE: variable of a reference type
                                    //string&local3;
                                    //// ISSUE: explicit reference operation
                                    //string str3 = ^(local3 = ref strArray[index1]) + Conversions.ToString(ch);
                                    //local3 = str3;
                                    continue;
                                }
                                switch (ch)
                                {
                                    case '.':
                                        int num3 = index2 + 1;
                                        int num4 = text.Length - 1;
                                        int index3 = num3;
                                        while (index3 <= num4 && text[index3] == '.')
                                            ++index3;
                                        int num5 = index3 - 1;
                                        if (num5 > index2)
                                        {
                                            if (initNotation == GH_NumberSlider.InitNotation.Range)
                                            {
                                                flag1 = false;
                                                goto label_51;
                                            }
                                            else
                                            {
                                                initNotation = GH_NumberSlider.InitNotation.DoubleDot;
                                                ++index1;
                                                if (index1 > 2)
                                                {
                                                    flag1 = false;
                                                    goto label_51;
                                                }
                                                else
                                                {
                                                    index2 = num5;
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                            break;
                                    case '<':
                                        if (initNotation == GH_NumberSlider.InitNotation.DoubleDot)
                                        {
                                            flag1 = false;
                                            goto label_51;
                                        }
                                        else
                                        {
                                            initNotation = GH_NumberSlider.InitNotation.Range;
                                            ++index1;
                                            if (index1 > 2)
                                            {
                                                flag1 = false;
                                                goto label_51;
                                            }
                                            else
                                                continue;
                                        }
                                }
                                //// ISSUE: variable of a reference type
                                //string&local4;
                                //// ISSUE: explicit reference operation
                                //string str4 = ^(local4 = ref strArray[index1]) + Conversions.ToString(ch);
                                //local4 = str4;
                                continue;
                        }
                    }
                }
                if (initNotation == GH_NumberSlider.InitNotation.None)
                    flag1 = false;
                else if (string.IsNullOrWhiteSpace(strArray[0]))
                {
                    flag1 = false;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(strArray[index1]))
                        --index1;
                    if (index1 < 0)
                    {
                        flag1 = false;
                    }
                    else
                    {
                        int num6 = strArray.Length - 1;
                        for (int index4 = 0; index4 <= num6; ++index4)
                        {
                            if (!string.IsNullOrWhiteSpace(strArray[index4]))
                            {
                                GH_Variant expression = GH_Convert.ParseExpression(strArray[index4], true);
                                if (expression.Type != GH_VariantType.@double && expression.Type != GH_VariantType.@int)
                                {
                                    flag1 = false;
                                    goto label_51;
                                }
                            }
                        }
                        switch (index1)
                        {
                            case 0:
                                double destination;
                                if (GH_Convert.ToDouble((object)strArray[0], out destination, GH_Conversion.Secondary))
                                {
                                    minimum = strArray[0].Trim();
                                    maximum = destination != 0.0 ? (destination >= 0.0 ? GH_Convert.ToNextPowerOfTen(destination).ToString() : GH_Convert.ToPrevPowerOfTen(destination).ToString()) : "1.0";
                                    value = strArray[0].Trim();
                                    break;
                                }
                                break;
                            case 1:
                                minimum = strArray[0].Trim();
                                maximum = strArray[1].Trim();
                                value = strArray[0].Trim();
                                break;
                            case 2:
                                minimum = strArray[0].Trim();
                                maximum = strArray[2].Trim();
                                value = strArray[1].Trim();
                                break;
                        }
                        flag1 = true;
                    }
                }
            }
        label_51:
            return flag1;
        }

        private void InternalSliderValueChanged(object sender, GH_SliderEventArgs e)
        {
            if (this.m_cache.Equals(e.Value))
                return;
            this.ResetCurrentValue();
            this.ExpireSolution(true);
        }

        public override void CreateAttributes() => this.m_attributes = (IGH_Attributes)new GH_NumberSliderAttributes(this);

        protected override void CollectVolatileData_Custom()
        {
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.m_data.Clear();
            if (!this.IsExpression)
            {
                this.m_data.Append(new GH_Number(Convert.ToDouble(this.CurrentValue)));
            }
            else
            {
                string Expression = GH_ExpressionSyntaxWriter.RewriteForEvaluator(this.m_expression);
                try
                {
                    GH_ExpressionParser expressionParser = new GH_ExpressionParser();
                    expressionParser.CacheSymbols(Expression);
                    expressionParser.ClearVariables();
                    expressionParser.AddVariable("x", Convert.ToDouble(this.Slider.Value));
                    GH_Variant ghVariant = expressionParser.Evaluate();
                    if (ghVariant.IsNumeric)
                    {
                        this.m_data.Append(new GH_Number(ghVariant._Double));
                        return;
                    }
                    this.m_data.Append(new GH_Number(double.NaN));
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Expression did not return a numeric value");
                    return;
                }
                catch (Exception ex)
                {
                    ProjectData.SetProjectError(ex);
                    ProjectData.ClearProjectError();
                }
                try
                {
                    GH_ExpressionParser expressionParser = new GH_ExpressionParser();
                    expressionParser.CacheSymbols(Expression);
                    expressionParser.ClearVariables();
                    expressionParser.AddVariable(this.NickName, Convert.ToDouble(this.Slider.Value));
                    GH_Variant ghVariant = expressionParser.Evaluate();
                    if (ghVariant.IsNumeric)
                    {
                        this.m_data.Append(new GH_Number(ghVariant._Double));
                    }
                    else
                    {
                        this.m_data.Append(new GH_Number(double.NaN));
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Expression did not return a numeric value");
                    }
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Expression uses Slider Name, please change it to \"x\"");
                }
                catch (Exception ex)
                {
                    ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    this.m_data.Append(new GH_Number(double.NaN));
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Expression generated an error: {0}", (object)exception.Message));
                    ProjectData.ClearProjectError();
                }
            }
        }

        /// <summary>
        /// Gets the internal slider instance. Do not muck about with this unless you know what you are doing.
        /// </summary>
        public GH_SliderBase Slider => this.m_slider;

        /// <summary>
        /// Gets the total number of ticks (unique positions) available on the slider.
        /// The number of ticks is limited to one billion (1,000,000,000)
        /// </summary>
        public int TickCount
        {
            get
            {
                int num1 = 1000000000;
                Decimal num2 = Decimal.Subtract(this.m_slider.Maximum, this.m_slider.Minimum);
                int num3 = this.Slider.Type != GH_SliderAccuracy.Float ? Convert.ToInt32(num2) : Convert.ToInt32(Convert.ToDouble(num2) * Math.Pow(10.0, (double)Math.Min(this.Slider.DecimalPlaces, 5)));
                return num3 < num1 ? Convert.ToInt32(num3) : num1;
            }
        }

        /// <summary>Gets or sets the current slider value as an offset in tick-space.</summary>
        public int TickValue
        {
            get
            {
                double normalizedValue = this.Slider.NormalizedValue;
                int tickCount = this.TickCount;
                double num = (double)tickCount;
                return Math.Min(Math.Max(Convert.ToInt32(normalizedValue * num), 0), tickCount);
            }
            set
            {
                int tickCount = this.TickCount;
                this.SetSliderValue(Decimal.Add(this.Slider.Minimum, Decimal.Multiply(Convert.ToDecimal((double)value / (double)tickCount), Decimal.Subtract(this.Slider.Maximum, this.Slider.Minimum))));
            }
        }

        /// <summary>
        /// Try and set the slider position in such a way as to result in the given value.
        /// If the slider contains an expression this expression will be back-solved (if possible).
        /// </summary>
        /// <param name="target">Target value to achieve.</param>
        /// <returns>True indicating success, false indicating failure.</returns>
        public bool TrySetSliderValue(Decimal target)
        {
            bool flag;
            if (!this.IsExpression)
            {
                this.Slider.Value = target;
                this.Slider.FixValue();
                flag = true;
            }
            else
            {
                double t = 0.0;
                double v = 0.0;
                if (!GH_Convert.BackSolveExpression(this.Expression, "x", Convert.ToDouble(target), Convert.ToDouble(this.Slider.Minimum), Convert.ToDouble(this.Slider.Maximum), 10, out t, out v))
                {
                    flag = false;
                }
                else
                {
                    this.Slider.Value = Convert.ToDecimal(t);
                    this.Slider.FixValue();
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// Sets member value directly.
        /// This sets the value of the internal slider, it does not backsolve for any expressions.
        /// </summary>
        /// <param name="val">Value to set.</param>
        public void SetSliderValue(Decimal val)
        {
            val = Math.Max(val, this.m_slider.Minimum);
            val = Math.Min(val, this.m_slider.Maximum);
            if (Decimal.Compare(this.Slider.Value, val) == 0)
                return;
            this.Slider.RaiseEvents = false;
            this.Slider.Value = val;
            this.Slider.RaiseEvents = true;
            this.ResetCurrentValue();
            this.ExpireSolution(false);
        }

        /// <summary>
        /// Gets the current slider value.
        /// This includes any expressions that might be active.
        /// Call this property if you want to know the current value.
        /// </summary>
        public Decimal CurrentValue
        {
            get
            {
                if (Decimal.Compare(this.m_cache, Decimal.MinValue) == 0)
                {
                    this.m_cache = this.Slider.Value;
                    if (this.IsExpression)
                    {
                        GH_ExpressionParser expressionParser = new GH_ExpressionParser(false);
                        try
                        {
                            expressionParser.CacheSymbols(this.Expression);
                            expressionParser.AddVariable("x", Convert.ToDouble(this.m_cache));
                            GH_Variant ghVariant = expressionParser.Evaluate();
                            switch (ghVariant.Type)
                            {
                                case GH_VariantType.@bool:
                                    this.m_cache = ghVariant._Bool ? 1M : 0M;
                                    break;
                                case GH_VariantType.@int:
                                    this.m_cache = new Decimal(ghVariant._Int);
                                    break;
                                case GH_VariantType.@double:
                                    this.m_cache = Convert.ToDecimal(ghVariant._Double);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ProjectData.SetProjectError(ex);
                            ProjectData.ClearProjectError();
                        }
                    }
                }
                return this.m_cache;
            }
        }

        /// <summary>
        /// Call this method if you think the currently cached value is not longer valid.
        /// </summary>
        public void ResetCurrentValue() => this.m_cache = Decimal.MinValue;

        /// <summary>Gets or sets the expression for this slider.</summary>
        public string Expression
        {
            get => this.m_expression;
            set
            {
                this.m_expression = !string.IsNullOrWhiteSpace(value) ? value : (string)null;
                this.ResetCurrentValue();
            }
        }

        /// <summary>Gets whether an expression has been assigned to this slider.</summary>
        public bool IsExpression => !string.IsNullOrWhiteSpace(this.m_expression);

        //protected internal override string HtmlHelp_Source()
        //{
        //    GH_HtmlFormatter ghHtmlFormatter = new GH_HtmlFormatter((IGH_InstanceDescription)this);
        //    ghHtmlFormatter.Title = "Number slider";
        //    ghHtmlFormatter.Description = "A slider is a special interface object that allows for quick setting of individual numeric values. You can change the values and properties through the menu, or by double-clicking a slider object. Sliders can be made longer or shorter by dragging the rightmost edge left or right. Note that sliders only have output grips. <BR> <BR>" + Environment.NewLine + "Sliders appear automatically in the Grasshopper Panel.";
        //    ghHtmlFormatter.ContactURI = "https://discourse.mcneel.com/";
        //    ghHtmlFormatter.AddRemark("Slider properties can be changed through the context menu or the slider editor. Double click the slider object (but not on the grip) to open the editor.");
        //    ghHtmlFormatter.AddRemark("Sliders can be used to generate an animation of the Grasshopper Document. The Animate… feature is available through the slider menu. The animation is a series of still images that represent the solution at different values for the slider. You can specify the resolution of the output, which viewport should be used, how many frames you want and even some tagging and filename formats. The numeric domain of the animation is always the numeric domain of the slider itself.");
        //    ghHtmlFormatter.AddRemark("At present it is not possible to render the animation, only viewport captures are possible.", GH_HtmlFormatterPalette.Red);
        //    return ghHtmlFormatter.HtmlFormat();
        //}

        public override bool IconCapableUI => false;

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            ToolStripMenuItem toolStripMenuItem1 = GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Slider type");
            GH_DocumentObject.Menu_AppendItem((ToolStrip)toolStripMenuItem1.DropDown, "Floating point", new EventHandler(this.Menu_FloatTypeClicked), true, this.Slider.Type == GH_SliderAccuracy.Float);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)toolStripMenuItem1.DropDown, "Integers", new EventHandler(this.Menu_IntegerTypeClicked), true, this.Slider.Type == GH_SliderAccuracy.Integer);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)toolStripMenuItem1.DropDown, "Odd numbers", new EventHandler(this.Menu_OddTypeClicked), true, this.Slider.Type == GH_SliderAccuracy.Odd);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)toolStripMenuItem1.DropDown, "Even numbers", new EventHandler(this.Menu_EvenTypeClicked), true, this.Slider.Type == GH_SliderAccuracy.Even);
            GH_DocumentObject.Menu_AppendSeparator((ToolStrip)menu);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Edit…", new EventHandler(this.Menu_EditClicked));
            GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Edit Snapping…", new EventHandler(this.Menu_EditSnappingClicked));
            this.Menu_AppendValueEditor(menu);
            string text = this.m_expression ?? string.Empty;
            ToolStripMenuItem toolStripMenuItem2 = GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Expression");
            GH_DocumentObject.Menu_AppendItem((ToolStrip)toolStripMenuItem2.DropDown, "Expression Editor", new EventHandler(this.Menu_ExpressionEditorClick), (Image)Res_ContextMenu.Modifier_Expression_16x16);
            GH_DocumentObject.Menu_AppendTextItem(toolStripMenuItem2.DropDown, text, new GH_MenuTextBox.KeyDownEventHandler(this.Menu_ExpressionItemKeyDown), (GH_MenuTextBox.TextChangedEventHandler)null, true, 200, true);
            GH_DocumentObject.Menu_AppendSeparator((ToolStrip)menu);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Animate…", new EventHandler(this.Menu_AnimateSlider));
        }

        private void Menu_FloatTypeClicked(object sender, EventArgs e)
        {
            if (this.Slider.Type == GH_SliderAccuracy.Float)
                return;
            this.Slider.Type = GH_SliderAccuracy.Float;
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.ExpireSolution(true);
        }

        private void Menu_IntegerTypeClicked(object sender, EventArgs e)
        {
            if (this.Slider.Type == GH_SliderAccuracy.Integer)
                return;
            this.Slider.Type = GH_SliderAccuracy.Integer;
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.ExpireSolution(true);
        }

        private void Menu_OddTypeClicked(object sender, EventArgs e)
        {
            if (this.Slider.Type == GH_SliderAccuracy.Odd)
                return;
            this.Slider.Type = GH_SliderAccuracy.Odd;
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.ExpireSolution(true);
        }

        private void Menu_EvenTypeClicked(object sender, EventArgs e)
        {
            if (this.Slider.Type == GH_SliderAccuracy.Even)
                return;
            this.Slider.Type = GH_SliderAccuracy.Even;
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.ExpireSolution(true);
        }

        private void Menu_EditClicked(object sender, EventArgs e) => this.PopupEditor();

        private void Menu_EditSnappingClicked(object sender, EventArgs e) => this.PopupSnappingEditor();

        private void Menu_ExpressionItemKeyDown(GH_MenuTextBox sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    string str = GH_ExpressionSyntaxWriter.RewriteForGraphicInterface(sender.Text);
                    sender.Text = str;
                    this.m_expression = str.Length != 0 ? str : (string)null;
                    this.ExpireSolution(true);
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    sender.CloseEntireMenuStructure();
                    e.Handled = true;
                    break;
            }
        }

        private void Menu_ExpressionEditorClick(object sender, EventArgs e)
        {
            GH_ExpressionEditor F = new GH_ExpressionEditor();
            F.PreviewDelegate = new GH_ExpressionEditor.GH_PreviewExpression(this.ExpressionEditorPreviewButtonClicked);
            F.Expression = this.m_expression;
            GH_Number ghNumber = this.m_data.get_FirstItem(true);
            if (ghNumber == null)
                F.Variables.Add("x", new GH_Variant(0.0));
            else
                F.Variables.Add("x", new GH_Variant(ghNumber.Value));
            GH_WindowsFormUtil.CenterFormOnCursor((Form)F, true);
            if (F.ShowDialog((IWin32Window)Instances.DocumentEditor) == DialogResult.OK)
                this.Expression = F.Expression;
            this.ExpireSolution(true);
        }

        private void ExpressionEditorPreviewButtonClicked(string sExpression)
        {
            string expression = this.m_expression;
            this.m_expression = sExpression;
            if (this.m_expression.Length == 0)
                this.m_expression = (string)null;
            this.ExpireSolution(true);
            this.m_expression = expression;
        }

        public void PopupEditor()
        {
            this.TriggerAutoSave();
            GH_NumberSliderPopup F = new GH_NumberSliderPopup();
            F.Setup(this);
            GH_WindowsFormUtil.CenterFormOnCursor((Form)F, true);
            if (F.ShowDialog((IWin32Window)Instances.DocumentEditor) != DialogResult.OK)
                return;
            this.Attributes.ExpireLayout();
            this.ExpireSolution(true);
        }

        public void PopupSnappingEditor()
        {
            this.TriggerAutoSave();
            GH_NumberSliderSnappingEditor F = new GH_NumberSliderSnappingEditor();
            F.SnappingTextBox.Text = string.Join(Environment.NewLine, (IEnumerable<string>)this._snapRanges);
            GH_WindowsFormUtil.CenterFormOnCursor((Form)F, true);
            if (F.ShowDialog((IWin32Window)Instances.DocumentEditor) != DialogResult.OK)
                return;
            this._snapRanges.Clear();
            this._snapRanges.AddRange((IEnumerable<string>)F.SnapLines);
            this.Slider.SetSnapRanges((IEnumerable<SliderSnapRange>)this.SnappingData);
            Instances.InvalidateCanvas();
        }

        private void Menu_AppendValueEditor(ToolStripDropDown menu)
        {
            ToolStripMenuItem toolStripMenuItem = GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Values");
            GH_NumericScrollerValueEditor scrollerValueEditor = new GH_NumericScrollerValueEditor();
            scrollerValueEditor.BackColor = SystemColors.Window;
            if (this.Slider.Type == GH_SliderAccuracy.Float)
                scrollerValueEditor.SetupValues(this.Slider.Minimum, this.Slider.Maximum, this.Slider.Value, this.Slider.DecimalPlaces);
            else
                scrollerValueEditor.SetupValues(this.Slider.Minimum, this.Slider.Maximum, this.Slider.Value, 0);
            scrollerValueEditor.OnLimitsChanged += new GH_NumericScrollerValueEditor.OnLimitsChangedEventHandler(this.Menu_NumLimitsChanged);
            scrollerValueEditor.OnValueChanged += new GH_NumericScrollerValueEditor.OnValueChangedEventHandler(this.Menu_NumValueChanged);
            GH_DocumentObject.Menu_AppendCustomItem(toolStripMenuItem.DropDown, (Control)scrollerValueEditor, new GH_MenuCustomControl.KeyDownEventHandler(this.Menu_NumValuesKeyDown), true, 200, true);
            GH_DocumentObject.Menu_AppendSeparator((ToolStrip)menu);
        }

        private void Menu_NumLimitsChanged(
          GH_NumericScrollerValueEditor sender,
          Decimal e_lower,
          Decimal e_upper,
          Decimal e_value)
        {
            this.Slider.RaiseEvents = false;
            this.Slider.Minimum = e_lower;
            this.Slider.Maximum = e_upper;
            this.Slider.Value = e_value;
            this.Slider.FixDomain();
            this.Slider.FixValue();
            this.Slider.RaiseEvents = true;
            this.Slider.OnValueChanged(false);
            Instances.RedrawCanvas();
        }

        private void Menu_NumValueChanged(
          GH_NumericScrollerValueEditor sender,
          Decimal e_lower,
          Decimal e_upper,
          Decimal e_value)
        {
            this.Slider.RaiseEvents = false;
            this.Slider.Minimum = e_lower;
            this.Slider.Maximum = e_upper;
            this.Slider.Value = e_value;
            this.Slider.RaiseEvents = true;
            this.Slider.OnValueChanged(false);
        }

        private void Menu_NumValuesKeyDown(GH_MenuCustomControl sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Cancel:
                case Keys.Escape:
                    this.Slider.OnValueChanged(false);
                    break;
                case Keys.Return:
                    this.Slider.OnValueChanged(false);
                    break;
            }
        }

        private void Menu_AnimateSlider(object sender, EventArgs e)
        {
            GH_SliderAnimator ghSliderAnimator = new GH_SliderAnimator(this);
            if (!ghSliderAnimator.SetupAnimationProperties())
                return;
            try
            {
                ghSliderAnimator.StartAnimation();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                Tracing.Assert(new Guid("{E04C96C0-74E5-4844-9233-5B66CC7C1024}"), "Slider animation failed: " + Environment.NewLine + ex.Message);
                ProjectData.ClearProjectError();
            }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override Bitmap Icon => Res_ObjectIcons.Obj_NumberSlider_24x24;

        public override string InstanceDescription
        {
            get
            {
                List<string> stringList = new List<string>();
                Decimal minimum = this.Slider.Minimum;
                Decimal maximum = this.Slider.Maximum;
                Decimal d1 = this.Slider.Value;
                Decimal num = Decimal.Multiply(100M, Decimal.Divide(Decimal.Subtract(d1, minimum), Decimal.Subtract(maximum, minimum)));
                switch (this.Slider.Type)
                {
                    case GH_SliderAccuracy.Float:
                        stringList.Add("Floating point accuracy");
                        break;
                    case GH_SliderAccuracy.Integer:
                        stringList.Add("Integer accuracy");
                        break;
                    case GH_SliderAccuracy.Even:
                        stringList.Add("Even number accuracy");
                        break;
                    case GH_SliderAccuracy.Odd:
                        stringList.Add("Odd number accuracy");
                        break;
                }
                stringList.Add(string.Format("Lower limit: {0}", (object)GH_NumberSlider.FormatNumber(minimum, this.Slider.Type, this.Slider.DecimalPlaces)));
                stringList.Add(string.Format("Upper limit: {0}", (object)GH_NumberSlider.FormatNumber(maximum, this.Slider.Type, this.Slider.DecimalPlaces)));
                stringList.Add(string.Format("Value: {0}", (object)GH_NumberSlider.FormatNumber(d1, this.Slider.Type, this.Slider.DecimalPlaces)));
                stringList.Add(string.Format("Factor: {0:0}%", (object)num));
                return string.Join(Environment.NewLine, stringList.ToArray());
            }
        }

        /// <summary>
        /// Convert a double precision floating point number into a string, using the rounding rules as indicated.
        /// </summary>
        /// <param name="value">Number to munge.</param>
        /// <param name="accuracy">Rounding algorithm to use.</param>
        /// <param name="digits">Number of digits, only applies for FloatingPoint rounding.</param>
        public static string FormatNumber(Decimal value, GH_SliderAccuracy accuracy, int digits) => accuracy != GH_SliderAccuracy.Float ? string.Format("{0:#0}", (object)value) : string.Format("{0:#0." + new string('0', digits) + "}", (object)value);

        /// <summary>
        /// Gets the implied nickname for this slider object.
        /// The implied nickname is just like the regular nickname,
        /// except when the regular nickname is a zero-length string and
        /// there are one or more recipient parameter, in which case the nickname
        /// is inherited from the recipients.
        /// </summary>
        public string ImpliedNickName
        {
            get
            {
                string impliedNickName;
                if (this.NickName.Length > 0)
                    impliedNickName = this.NickName;
                else if (this.Recipients.Count == 0)
                {
                    impliedNickName = "Slider";
                }
                else
                {
                    string str = "Slider";
                    double num1 = double.MaxValue;
                    int num2 = this.Recipients.Count - 1;
                    for (int index = 0; index <= num2; ++index)
                    {
                        double num3 = (double)GH_GraphicsUtil.DistanceS(this.Attributes.OutputGrip, this.Recipients[index].Attributes.InputGrip);
                        if (num3 < num1)
                        {
                            num1 = num3;
                            str = this.Recipients[index].Name;
                        }
                    }
                    impliedNickName = str;
                }
                return impliedNickName;
            }
        }

        public override GH_ParamKind Kind => GH_ParamKind.floating;

        public override GH_ParamData DataType => GH_ParamData.local;

        public override string TypeName => "Number";

        /// <summary>Gets the Slider Component Guid.</summary>
        public static Guid SliderGuid => new Guid("{57DA07BD-ECAB-415d-9D86-AF36D7073ABC}");

        public override Guid ComponentGuid => GH_NumberSlider.SliderGuid;

        public override bool Write(GH_IWriter writer)
        {
            bool flag = base.Write(writer);
            GH_IWriter chunk = writer.CreateChunk("Slider");
            chunk.SetDouble("Value", Convert.ToDouble(this.Slider.Value));
            chunk.SetDouble("Min", Convert.ToDouble(this.Slider.Minimum));
            chunk.SetDouble("Max", Convert.ToDouble(this.Slider.Maximum));
            chunk.SetInt32("Digits", this.Slider.DecimalPlaces);
            switch (this.Slider.Type)
            {
                case GH_SliderAccuracy.Float:
                    chunk.SetInt32("Interval", 0);
                    break;
                case GH_SliderAccuracy.Integer:
                    chunk.SetInt32("Interval", 1);
                    break;
                case GH_SliderAccuracy.Even:
                    chunk.SetInt32("Interval", 3);
                    break;
                case GH_SliderAccuracy.Odd:
                    chunk.SetInt32("Interval", 2);
                    break;
            }
            switch (this.Slider.GripDisplay)
            {
                case GH_SliderGripDisplay.Numeric:
                    chunk.SetInt32("GripDisplay", 2);
                    break;
                case GH_SliderGripDisplay.Shape:
                    chunk.SetInt32("GripDisplay", 0);
                    break;
                case GH_SliderGripDisplay.ShapeAndText:
                    chunk.SetInt32("GripDisplay", 1);
                    break;
            }
            if (!string.IsNullOrEmpty(this.m_expression))
                chunk.SetString("Expression", this.m_expression);
            chunk.SetInt32("SnapCount", this._snapRanges.Count);
            if (this._snapRanges.Count > 0)
            {
                int num = this._snapRanges.Count - 1;
                for (int index = 0; index <= num; ++index)
                    chunk.SetString("Snap", index, this._snapRanges[index]);
            }
            return flag;
        }

        public override bool Read(GH_IReader reader)
        {
            bool flag;
            if (!base.Read(reader))
            {
                flag = false;
            }
            else
            {
                GH_IReader chunk = reader.FindChunk("Slider");
                if (chunk == null)
                {
                    reader.AddMessage("Slider property chunk is missing, archive is corrupt.", GH_Message_Type.error);
                    flag = false;
                }
                else
                {
                    this.Slider.DecimalPlaces = chunk.GetInt32("Digits");
                    this.Slider.Minimum = Convert.ToDecimal(chunk.GetDouble("Min"));
                    this.Slider.Maximum = Convert.ToDecimal(chunk.GetDouble("Max"));
                    this.Slider.Value = Convert.ToDecimal(chunk.GetDouble("Value"));
                    this.m_expression = !chunk.ItemExists("Expression") ? (string)null : chunk.GetString("Expression");
                    switch (chunk.GetInt32("Interval"))
                    {
                        case 0:
                            this.Slider.Type = GH_SliderAccuracy.Float;
                            break;
                        case 1:
                            this.Slider.Type = GH_SliderAccuracy.Integer;
                            break;
                        case 2:
                            this.Slider.Type = GH_SliderAccuracy.Odd;
                            break;
                        case 3:
                            this.Slider.Type = GH_SliderAccuracy.Even;
                            break;
                        default:
                            reader.AddMessage("Slider domain type is unrecognized.", GH_Message_Type.warning);
                            this.Slider.Type = GH_SliderAccuracy.Float;
                            break;
                    }
                    this.Slider.GripDisplay = GH_SliderGripDisplay.ShapeAndText;
                    if (chunk.ItemExists("GripDisplay"))
                    {
                        switch (chunk.GetInt32("GripDisplay"))
                        {
                            case 0:
                                this.Slider.GripDisplay = GH_SliderGripDisplay.Shape;
                                break;
                            case 1:
                                this.Slider.GripDisplay = GH_SliderGripDisplay.ShapeAndText;
                                break;
                            case 2:
                                this.Slider.GripDisplay = GH_SliderGripDisplay.Numeric;
                                break;
                            default:
                                this.Slider.GripDisplay = GH_SliderGripDisplay.ShapeAndText;
                                break;
                        }
                    }
                    this.Slider.FixDomain();
                    this.Slider.FixValue();
                    this.ResetCurrentValue();
                    this._snapRanges.Clear();
                    if (chunk.ItemExists("SnapCount"))
                    {
                        int num = chunk.GetInt32("SnapCount") - 1;
                        for (int item_index = 0; item_index <= num; ++item_index)
                            this._snapRanges.Add(chunk.GetString("Snap", item_index));
                    }
                    this.Slider.SetSnapRanges((IEnumerable<SliderSnapRange>)this.SnappingData);
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// Gets all valid sticky values as intervals.
        /// This automatically adds errors to the slider if a sticky text isn't valid.
        /// </summary>
        private List<SliderSnapRange> SnappingData
        {
            get
            {
                List<SliderSnapRange> snappingData = new List<SliderSnapRange>();
                try
                {
                    foreach (string snapRange in this._snapRanges)
                    {
                        if (snapRange.ToUpperInvariant().Contains("TO"))
                        {
                            Interval rc;
                            if (GH_Convert.ToInterval((object)snapRange, ref rc, GH_Conversion.Both))
                                snappingData.Add(new SliderSnapRange(Convert.ToDecimal(((Interval)ref rc).Min), Convert.ToDecimal(((Interval)ref rc).Max)));
                            else
                                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Sticky value '{0}' is not a valid range", (object)snapRange));
                        }
                        else
                        {
                            double destination;
                            if (GH_Convert.ToDouble((object)snapRange, out destination, GH_Conversion.Both))
                                snappingData.Add(new SliderSnapRange(Convert.ToDecimal(destination)));
                            else
                                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Sticky value '{0}' is not a valid number", (object)snapRange));
                        }
                    }
                }
                finally
                {
                    List<string>.Enumerator enumerator;
                    enumerator.Dispose();
                }
                return snappingData;
            }
        }

        public string SaveState()
        {
            GH_LooseChunk ghLooseChunk = new GH_LooseChunk("slider");
            ghLooseChunk.SetDouble("Minimum", Convert.ToDouble(this.Slider.Minimum));
            ghLooseChunk.SetDouble("Maximum", Convert.ToDouble(this.Slider.Maximum));
            ghLooseChunk.SetInt32("Digits", this.Slider.DecimalPlaces);
            ghLooseChunk.SetDouble("Value", Convert.ToDouble(this.Slider.Value));
            return ghLooseChunk.Serialize_Xml();
        }

        public void LoadState(string state)
        {
            GH_LooseChunk ghLooseChunk = new GH_LooseChunk("slider");
            ghLooseChunk.Deserialize_Xml(state);
            double num1 = Convert.ToDouble(this.Slider.Minimum);
            ghLooseChunk.TryGetDouble("Minimum", ref num1);
            double num2 = Convert.ToDouble(this.Slider.Maximum);
            ghLooseChunk.TryGetDouble("Maximum", ref num2);
            int decimalPlaces = this.Slider.DecimalPlaces;
            ghLooseChunk.TryGetInt32("Digits", ref decimalPlaces);
            double num3 = Convert.ToDouble(this.Slider.Value);
            ghLooseChunk.TryGetDouble("Value", ref num3);
            this.Slider.RaiseEvents = false;
            this.Slider.Minimum = Convert.ToDecimal(num1);
            this.Slider.Maximum = Convert.ToDecimal(num2);
            this.Slider.DecimalPlaces = decimalPlaces;
            this.Slider.Value = Convert.ToDecimal(num3);
            this.Slider.RaiseEvents = true;
            this.ResetCurrentValue();
            this.ExpireSolution(false);
        }

        public IRcpItem PublishRcpItem() => (IRcpItem)new GH_NumberSlider.GH_NumberSliderPublishProxy(this);

        private enum InitNotation
        {
            None,
            DoubleDot,
            Range,
        }

        public class GH_NumberSliderPublishProxy : RcpDocumentObjectItem<GH_NumberSlider>
        {
            private readonly GH_SliderBase _slider;
            private bool _down;

            public GH_NumberSliderPublishProxy(GH_NumberSlider owner)
              : base(owner)
            {
                this._slider = new GH_SliderBase();
                this._down = false;
            }

            public override int DesiredHeight => Global_Proc.UiAdjust(20);

            public override void Render(Graphics G)
            {
                if (this.Owner == null)
                {
                    G.SmoothingMode = SmoothingMode.None;
                    Pen pen = new Pen(RcpItem.ErrorHint, 2f);
                    G.DrawRectangle(pen, this.ClientBounds);
                    pen.Dispose();
                    SolidBrush solidBrush = new SolidBrush(RcpItem.ErrorHint);
                    G.DrawString("Slider missing", GH_FontServer.StandardBold, (Brush)solidBrush, (RectangleF)this.ClientBounds, GH_TextRenderingConstants.NearCenter);
                    solidBrush.Dispose();
                }
                else
                {
                    SolidBrush solidBrush1 = new SolidBrush(RcpItem.FieldFill);
                    G.FillRectangle((Brush)solidBrush1, this.ClientBounds);
                    solidBrush1.Dispose();
                    Pen pen = new Pen(RcpItem.NormalEdge, 2f);
                    G.DrawRectangle(pen, this.ClientBounds);
                    pen.Dispose();
                    Rectangle clientBounds = this.ClientBounds;
                    clientBounds.Inflate(0, 2);
                    this._slider.Bounds = clientBounds;
                    this._slider.Type = this.Owner.Slider.Type;
                    this._slider.FormatMask = this.Owner.Slider.FormatMask;
                    this._slider.Minimum = this.Owner.Slider.Minimum;
                    this._slider.Maximum = this.Owner.Slider.Maximum;
                    this._slider.DecimalPlaces = this.Owner.Slider.DecimalPlaces;
                    this._slider.Value = this.Owner.Slider.Value;
                    this._slider.DrawControlBackground = false;
                    this._slider.DrawControlBorder = false;
                    this._slider.GripDisplay = GH_SliderGripDisplay.Numeric;
                    this._slider.RailDisplay = GH_SliderRailDisplay.None;
                    this._slider.TickDisplay = GH_SliderTickDisplay.None;
                    this._slider.DrawControlShadows = false;
                    Color color1 = RcpItem.NormalFill;
                    Color color2 = RcpItem.NormalEdge;
                    if (this._down)
                    {
                        color1 = RcpItem.ActiveFill;
                        color2 = RcpItem.ActiveEdge;
                    }
                    this._slider.GripTopColour = color1;
                    this._slider.GripBottomColour = color1;
                    this._slider.GripEdgeColour = color2;
                    this._slider.TextColour = color2;
                    string impliedNickName = this.Owner.ImpliedNickName;
                    if (!string.IsNullOrWhiteSpace(impliedNickName))
                    {
                        SolidBrush solidBrush2 = new SolidBrush(RcpItem.NormalEdge);
                        if ((double)(this.TextBounds.X + GH_FontServer.StringWidth(impliedNickName, GH_FontServer.StandardBold) + 10) < (double)this._slider.Grip.Left)
                            G.DrawString(impliedNickName, GH_FontServer.StandardBold, (Brush)solidBrush2, (RectangleF)this.TextBounds, GH_TextRenderingConstants.NearCenter);
                        else
                            G.DrawString(impliedNickName, GH_FontServer.StandardBold, (Brush)solidBrush2, (RectangleF)this.TextBounds, GH_TextRenderingConstants.FarCenter);
                        solidBrush2.Dispose();
                    }
                    this._slider.Render(G);
                }
                G.SmoothingMode = SmoothingMode.HighQuality;
            }

            private void SliderValueChanged(object sender, GH_SliderEventArgs e)
            {
                this._slider.ValueChanged -= new GH_SliderBase.ValueChangedEventHandler(this.SliderValueChanged);
                if (this.Owner == null)
                    return;
                this.Owner.Slider.Value = e.Slider.Value;
                this.Owner.ExpireSolution(true);
            }

            public override GH_ObjectResponse MouseDown(MouseEventArgs e)
            {
                GH_ObjectResponse ghObjectResponse;
                if (this._slider.MouseDown(e, (PointF)e.Location))
                {
                    this._down = true;
                    ghObjectResponse = GH_ObjectResponse.Capture;
                }
                else
                    ghObjectResponse = base.MouseDown(e);
                return ghObjectResponse;
            }

            public override GH_ObjectResponse MouseMove(MouseEventArgs e)
            {
                this._slider.ValueChanged += new GH_SliderBase.ValueChangedEventHandler(this.SliderValueChanged);
                int num = this._slider.MouseMove(e, (PointF)e.Location) ? 1 : 0;
                this._slider.ValueChanged -= new GH_SliderBase.ValueChangedEventHandler(this.SliderValueChanged);
                return num == 0 ? base.MouseMove(e) : GH_ObjectResponse.Capture;
            }

            public override GH_ObjectResponse MouseUp(MouseEventArgs e)
            {
                this._down = false;
                return !this._slider.MouseUp(e, (PointF)e.Location) ? base.MouseUp(e) : GH_ObjectResponse.Release;
            }
        }
    }
}
