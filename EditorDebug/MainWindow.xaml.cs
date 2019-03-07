using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BaseNode;
using BaseNodeHelper;
using EaslyController.Layout;
using Microsoft.Win32;
using PolySerializer;

namespace EditorDebug
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Init
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            int Value;
            Debug.Assert(IntegerBase.Binary.IsValidDigit('0', out Value) && Value == 0);
            Debug.Assert(IntegerBase.Binary.IsValidDigit('1', out Value) && Value == 1);
            Debug.Assert(!IntegerBase.Binary.IsValidDigit('2', out Value));
            Debug.Assert(IntegerBase.Octal.IsValidDigit('0', out Value) && Value == 0);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('1', out Value) && Value == 1);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('2', out Value) && Value == 2);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('3', out Value) && Value == 3);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('4', out Value) && Value == 4);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('5', out Value) && Value == 5);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('6', out Value) && Value == 6);
            Debug.Assert(IntegerBase.Octal.IsValidDigit('7', out Value) && Value == 7);
            Debug.Assert(!IntegerBase.Octal.IsValidDigit('8', out Value));
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('0', out Value) && Value == 0);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('1', out Value) && Value == 1);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('2', out Value) && Value == 2);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('3', out Value) && Value == 3);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('4', out Value) && Value == 4);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('5', out Value) && Value == 5);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('6', out Value) && Value == 6);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('7', out Value) && Value == 7);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('8', out Value) && Value == 8);
            Debug.Assert(IntegerBase.Decimal.IsValidDigit('9', out Value) && Value == 9);
            Debug.Assert(!IntegerBase.Decimal.IsValidDigit('a', out Value));
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('0', out Value) && Value == 0);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('1', out Value) && Value == 1);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('2', out Value) && Value == 2);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('3', out Value) && Value == 3);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('4', out Value) && Value == 4);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('5', out Value) && Value == 5);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('6', out Value) && Value == 6);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('7', out Value) && Value == 7);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('8', out Value) && Value == 8);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('9', out Value) && Value == 9);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('a', out Value) && Value == 10);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('b', out Value) && Value == 11);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('c', out Value) && Value == 12);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('d', out Value) && Value == 13);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('e', out Value) && Value == 14);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('f', out Value) && Value == 15);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('A', out Value) && Value == 10);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('B', out Value) && Value == 11);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('C', out Value) && Value == 12);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('D', out Value) && Value == 13);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('E', out Value) && Value == 14);
            Debug.Assert(IntegerBase.Hexadecimal.IsValidDigit('F', out Value) && Value == 15);
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidDigit('g', out Value));
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidDigit('G', out Value));

            Debug.Assert(!IntegerBase.Binary.IsValidNumber(""));
            Debug.Assert(IntegerBase.Binary.IsValidNumber("0"));
            Debug.Assert(IntegerBase.Binary.IsValidNumber("1"));
            Debug.Assert(!IntegerBase.Binary.IsValidNumber("2"));
            Debug.Assert(!IntegerBase.Binary.IsValidNumber("0120"));
            Debug.Assert(!IntegerBase.Binary.IsValidNumber("010"));
            Debug.Assert(!IntegerBase.Binary.IsValidNumber("011"));
            Debug.Assert(IntegerBase.Binary.IsValidNumber("111"));
            Debug.Assert(IntegerBase.Binary.IsValidNumber("110"));
            Debug.Assert(IntegerBase.Binary.IsValidSignificand("111"));
            Debug.Assert(!IntegerBase.Binary.IsValidSignificand("110"));

            Debug.Assert(!IntegerBase.Octal.IsValidNumber(""));
            Debug.Assert(IntegerBase.Octal.IsValidNumber("0"));
            Debug.Assert(IntegerBase.Octal.IsValidNumber("1"));
            Debug.Assert(!IntegerBase.Octal.IsValidNumber("8"));
            Debug.Assert(!IntegerBase.Octal.IsValidNumber("0180"));
            Debug.Assert(!IntegerBase.Octal.IsValidNumber("010"));
            Debug.Assert(!IntegerBase.Octal.IsValidNumber("011"));
            Debug.Assert(IntegerBase.Octal.IsValidNumber("111"));
            Debug.Assert(IntegerBase.Octal.IsValidNumber("110"));
            Debug.Assert(IntegerBase.Octal.IsValidSignificand("111"));
            Debug.Assert(!IntegerBase.Octal.IsValidSignificand("110"));

            Debug.Assert(!IntegerBase.Decimal.IsValidNumber(""));
            Debug.Assert(IntegerBase.Decimal.IsValidNumber("0"));
            Debug.Assert(IntegerBase.Decimal.IsValidNumber("1"));
            Debug.Assert(!IntegerBase.Decimal.IsValidNumber("a"));
            Debug.Assert(!IntegerBase.Decimal.IsValidNumber("01a0"));
            Debug.Assert(!IntegerBase.Decimal.IsValidNumber("010"));
            Debug.Assert(!IntegerBase.Decimal.IsValidNumber("011"));
            Debug.Assert(IntegerBase.Decimal.IsValidNumber("111"));
            Debug.Assert(IntegerBase.Decimal.IsValidNumber("110"));
            Debug.Assert(IntegerBase.Decimal.IsValidSignificand("111"));
            Debug.Assert(!IntegerBase.Decimal.IsValidSignificand("110"));

            Debug.Assert(!IntegerBase.Hexadecimal.IsValidNumber(""));
            Debug.Assert(IntegerBase.Hexadecimal.IsValidNumber("0"));
            Debug.Assert(IntegerBase.Hexadecimal.IsValidNumber("1"));
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidNumber("g"));
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidNumber("01g0"));
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidNumber("010"));
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidNumber("011"));
            Debug.Assert(IntegerBase.Hexadecimal.IsValidNumber("111"));
            Debug.Assert(IntegerBase.Hexadecimal.IsValidNumber("110"));
            Debug.Assert(IntegerBase.Hexadecimal.IsValidSignificand("111"));
            Debug.Assert(!IntegerBase.Hexadecimal.IsValidSignificand("110"));

            Debug.Assert(IntegerBase.Binary.ToDigit(0) == '0');
            Debug.Assert(IntegerBase.Binary.ToDigit(1) == '1');

            Debug.Assert(IntegerBase.Octal.ToDigit(0) == '0');
            Debug.Assert(IntegerBase.Octal.ToDigit(1) == '1');
            Debug.Assert(IntegerBase.Octal.ToDigit(2) == '2');
            Debug.Assert(IntegerBase.Octal.ToDigit(3) == '3');
            Debug.Assert(IntegerBase.Octal.ToDigit(4) == '4');
            Debug.Assert(IntegerBase.Octal.ToDigit(5) == '5');
            Debug.Assert(IntegerBase.Octal.ToDigit(6) == '6');
            Debug.Assert(IntegerBase.Octal.ToDigit(7) == '7');

            Debug.Assert(IntegerBase.Decimal.ToDigit(0) == '0');
            Debug.Assert(IntegerBase.Decimal.ToDigit(1) == '1');
            Debug.Assert(IntegerBase.Decimal.ToDigit(2) == '2');
            Debug.Assert(IntegerBase.Decimal.ToDigit(3) == '3');
            Debug.Assert(IntegerBase.Decimal.ToDigit(4) == '4');
            Debug.Assert(IntegerBase.Decimal.ToDigit(5) == '5');
            Debug.Assert(IntegerBase.Decimal.ToDigit(6) == '6');
            Debug.Assert(IntegerBase.Decimal.ToDigit(7) == '7');
            Debug.Assert(IntegerBase.Decimal.ToDigit(8) == '8');
            Debug.Assert(IntegerBase.Decimal.ToDigit(9) == '9');

            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(0) == '0');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(1) == '1');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(2) == '2');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(3) == '3');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(4) == '4');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(5) == '5');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(6) == '6');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(7) == '7');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(8) == '8');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(9) == '9');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(10) == 'A');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(11) == 'B');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(12) == 'C');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(13) == 'D');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(14) == 'E');
            Debug.Assert(IntegerBase.Hexadecimal.ToDigit(15) == 'F');


            Debug.Assert(IntegerBase.Binary.ToValue('0') == 0);
            Debug.Assert(IntegerBase.Binary.ToValue('1') == 1);

            Debug.Assert(IntegerBase.Octal.ToValue('0') == 0);
            Debug.Assert(IntegerBase.Octal.ToValue('1') == 1);
            Debug.Assert(IntegerBase.Octal.ToValue('2') == 2);
            Debug.Assert(IntegerBase.Octal.ToValue('3') == 3);
            Debug.Assert(IntegerBase.Octal.ToValue('4') == 4);
            Debug.Assert(IntegerBase.Octal.ToValue('5') == 5);
            Debug.Assert(IntegerBase.Octal.ToValue('6') == 6);
            Debug.Assert(IntegerBase.Octal.ToValue('7') == 7);

            Debug.Assert(IntegerBase.Decimal.ToValue('0') == 0);
            Debug.Assert(IntegerBase.Decimal.ToValue('1') == 1);
            Debug.Assert(IntegerBase.Decimal.ToValue('2') == 2);
            Debug.Assert(IntegerBase.Decimal.ToValue('3') == 3);
            Debug.Assert(IntegerBase.Decimal.ToValue('4') == 4);
            Debug.Assert(IntegerBase.Decimal.ToValue('5') == 5);
            Debug.Assert(IntegerBase.Decimal.ToValue('6') == 6);
            Debug.Assert(IntegerBase.Decimal.ToValue('7') == 7);
            Debug.Assert(IntegerBase.Decimal.ToValue('8') == 8);
            Debug.Assert(IntegerBase.Decimal.ToValue('9') == 9);

            Debug.Assert(IntegerBase.Hexadecimal.ToValue('0') == 0);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('1') == 1);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('2') == 2);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('3') == 3);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('4') == 4);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('5') == 5);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('6') == 6);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('7') == 7);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('8') == 8);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('9') == 9);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('a') == 10);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('b') == 11);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('c') == 12);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('d') == 13);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('e') == 14);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('f') == 15);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('A') == 10);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('B') == 11);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('C') == 12);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('D') == 13);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('E') == 14);
            Debug.Assert(IntegerBase.Hexadecimal.ToValue('F') == 15);

            bool HasCarry;

            Debug.Assert(IntegerBase.Binary.DividedByTwo("0", out HasCarry) == "0" && !HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("1", out HasCarry) == "0" && HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("10", out HasCarry) == "1" && !HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("11", out HasCarry) == "1" && HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("100", out HasCarry) == "10" && !HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("101", out HasCarry) == "10" && HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("110", out HasCarry) == "11" && !HasCarry);
            Debug.Assert(IntegerBase.Binary.DividedByTwo("111", out HasCarry) == "11" && HasCarry);

            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("0", false) == "0");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("0", true) == "1");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("1", false) == "10");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("1", true) == "11");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("10", false) == "100");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("10", true) == "101");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("11", false) == "110");
            Debug.Assert(IntegerBase.Binary.MultipliedByTwo("11", true) == "111");

            Debug.Assert(IntegerBase.Octal.DividedByTwo("0", out HasCarry) == "0" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("1", out HasCarry) == "0" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("2", out HasCarry) == "1" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("3", out HasCarry) == "1" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("4", out HasCarry) == "2" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("5", out HasCarry) == "2" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("6", out HasCarry) == "3" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("7", out HasCarry) == "3" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("10", out HasCarry) == "4" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("11", out HasCarry) == "4" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("12", out HasCarry) == "5" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("13", out HasCarry) == "5" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("14", out HasCarry) == "6" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("15", out HasCarry) == "6" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("16", out HasCarry) == "7" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("17", out HasCarry) == "7" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("20", out HasCarry) == "10" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("21", out HasCarry) == "10" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("22", out HasCarry) == "11" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("23", out HasCarry) == "11" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("24", out HasCarry) == "12" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("25", out HasCarry) == "12" && HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("26", out HasCarry) == "13" && !HasCarry);
            Debug.Assert(IntegerBase.Octal.DividedByTwo("27", out HasCarry) == "13" && HasCarry);

            Random Rand = new Random(0);
            int MaxDigits = 30;

            string BinaryNumber = "";
            string OctalNumber = "";
            string DecimalNumber = "";
            string HexadecimalNumber = "";

            for (int i = 0; i < MaxDigits; i++)
            {
                int BinaryValue = Rand.Next(IntegerBase.Binary.Radix);
                do BinaryValue = Rand.Next(IntegerBase.Binary.Radix); while (i + 1 == MaxDigits && BinaryValue == 0);
                char BinaryDigit = IntegerBase.Binary.ToDigit(BinaryValue);
                BinaryNumber += BinaryDigit;

                int OctalValue = Rand.Next(IntegerBase.Octal.Radix);
                do OctalValue = Rand.Next(IntegerBase.Octal.Radix); while (i + 1 == MaxDigits && OctalValue == 0);
                char OctalDigit = IntegerBase.Octal.ToDigit(OctalValue);
                OctalNumber += OctalDigit;

                int DecimalValue = Rand.Next(IntegerBase.Decimal.Radix);
                do DecimalValue = Rand.Next(IntegerBase.Decimal.Radix); while (i + 1 == MaxDigits && DecimalValue == 0);
                char DecimalDigit = IntegerBase.Decimal.ToDigit(DecimalValue);
                DecimalNumber += DecimalDigit;

                int HexadecimalValue = Rand.Next(IntegerBase.Hexadecimal.Radix);
                do HexadecimalValue = Rand.Next(IntegerBase.Hexadecimal.Radix); while (i + 1 == MaxDigits && HexadecimalValue == 0);
                char HexadecimalDigit = IntegerBase.Hexadecimal.ToDigit(HexadecimalValue);
                HexadecimalNumber += HexadecimalDigit;
            }

            string BinaryNumberDivided = IntegerBase.Binary.DividedByTwo(BinaryNumber, out HasCarry);
            string BinaryNumberRemultiplied = IntegerBase.Binary.MultipliedByTwo(BinaryNumberDivided, HasCarry);
            Debug.Assert(BinaryNumberRemultiplied == BinaryNumber);

            string OctalNumberDivided = IntegerBase.Octal.DividedByTwo(OctalNumber, out HasCarry);
            string OctalNumberRemultiplied = IntegerBase.Octal.MultipliedByTwo(OctalNumberDivided, HasCarry);
            Debug.Assert(OctalNumberRemultiplied == OctalNumber);

            string DecimalNumberDivided = IntegerBase.Decimal.DividedByTwo(DecimalNumber, out HasCarry);
            string DecimalNumberRemultiplied = IntegerBase.Decimal.MultipliedByTwo(DecimalNumberDivided, HasCarry);
            Debug.Assert(DecimalNumberRemultiplied == DecimalNumber);

            string HexadecimalNumberDivided = IntegerBase.Hexadecimal.DividedByTwo(HexadecimalNumber, out HasCarry);
            string HexadecimalNumberRemultiplied = IntegerBase.Hexadecimal.MultipliedByTwo(HexadecimalNumberDivided, HasCarry);
            Debug.Assert(HexadecimalNumberRemultiplied == HexadecimalNumber);

            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(BinaryNumber, IntegerBase.Binary, IntegerBase.Binary), IntegerBase.Binary, IntegerBase.Binary) == BinaryNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(BinaryNumber, IntegerBase.Binary, IntegerBase.Octal), IntegerBase.Octal, IntegerBase.Binary) == BinaryNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(BinaryNumber, IntegerBase.Binary, IntegerBase.Decimal), IntegerBase.Decimal, IntegerBase.Binary) == BinaryNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(BinaryNumber, IntegerBase.Binary, IntegerBase.Hexadecimal), IntegerBase.Hexadecimal, IntegerBase.Binary) == BinaryNumber);

            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(OctalNumber, IntegerBase.Octal, IntegerBase.Binary), IntegerBase.Binary, IntegerBase.Octal) == OctalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(OctalNumber, IntegerBase.Octal, IntegerBase.Octal), IntegerBase.Octal, IntegerBase.Octal) == OctalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(OctalNumber, IntegerBase.Octal, IntegerBase.Decimal), IntegerBase.Decimal, IntegerBase.Octal) == OctalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(OctalNumber, IntegerBase.Octal, IntegerBase.Hexadecimal), IntegerBase.Hexadecimal, IntegerBase.Octal) == OctalNumber);

            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(DecimalNumber, IntegerBase.Decimal, IntegerBase.Binary), IntegerBase.Binary, IntegerBase.Decimal) == DecimalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(DecimalNumber, IntegerBase.Decimal, IntegerBase.Octal), IntegerBase.Octal, IntegerBase.Decimal) == DecimalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(DecimalNumber, IntegerBase.Decimal, IntegerBase.Decimal), IntegerBase.Decimal, IntegerBase.Decimal) == DecimalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(DecimalNumber, IntegerBase.Decimal, IntegerBase.Hexadecimal), IntegerBase.Hexadecimal, IntegerBase.Decimal) == DecimalNumber);

            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(HexadecimalNumber, IntegerBase.Hexadecimal, IntegerBase.Binary), IntegerBase.Binary, IntegerBase.Hexadecimal) == HexadecimalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(HexadecimalNumber, IntegerBase.Hexadecimal, IntegerBase.Octal), IntegerBase.Octal, IntegerBase.Hexadecimal) == HexadecimalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(HexadecimalNumber, IntegerBase.Hexadecimal, IntegerBase.Decimal), IntegerBase.Decimal, IntegerBase.Hexadecimal) == HexadecimalNumber);
            Debug.Assert(IntegerBase.Convert(IntegerBase.Convert(HexadecimalNumber, IntegerBase.Hexadecimal, IntegerBase.Hexadecimal), IntegerBase.Hexadecimal, IntegerBase.Hexadecimal) == HexadecimalNumber);

            IFormattedNumber Number;
            Number = FormattedNumber.Parse("");
            Number = FormattedNumber.Parse("0");
            Number = FormattedNumber.Parse("0:B");
            Number = FormattedNumber.Parse("0:O");
            Number = FormattedNumber.Parse("0:H");
            Number = FormattedNumber.Parse("5");
            Number = FormattedNumber.Parse("1:B");
            Number = FormattedNumber.Parse("5:O");
            Number = FormattedNumber.Parse("F:H");
            Number = FormattedNumber.Parse("468F3ECF:H");
            Number = FormattedNumber.Parse("468F3xECF:H");

            string Charset = "01.e-+";
            //string Pattern = "01.01e-00";
            long N = Charset.Length;
            long T = N * N * N * N * N * N * N * N * N;
            Debug.WriteLine($"T = {T}");
            double Percent = 1.0;
            for (long n = 0; n < T; n++)
            {
                string s = GenerateNumber(Charset, n);
                Number = FormattedNumber.Parse(s);
                double d = (100.0 * ((double)n)) / ((double)T);
                if (d >= Percent)
                {
                    Debug.WriteLine(((int)Percent).ToString() + "%");
                    Percent += 1.0;
                }
            }

            CurrentDirectory = Environment.CurrentDirectory;
            if (CurrentDirectory.Contains("Debug") || CurrentDirectory.Contains("Release"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("x64"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("bin"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);

            CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            CurrentDirectory = Path.Combine(CurrentDirectory, "Test");

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //MaxWidth = ActualWidth;
            //MaxHeight = ActualHeight;
        }

        private void OnBrowse(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = CurrentDirectory;

            bool? Result = Dlg.ShowDialog(this);
            if (Result.HasValue && Result.Value)
            {
                LoadFile(Dlg.FileName);
                bool b = layoutControl.Focus();
            }
        }

        private void LoadFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                CurrentFileName = fileName;
                NotifyPropertyChanged(nameof(CurrentFileName));

                Serializer Serializer = new Serializer();
                INode RootNode = Serializer.Deserialize(fs) as INode;
                LoadFileLayout(RootNode);
            }
        }

        public string CurrentDirectory { get; private set; }
        public string CurrentFileName { get; private set; }
        ILayoutController Controller;

        private string GenerateNumber(string charset, long pattern)
        {
            if (pattern == 0)
                return "";

            string s = charset.Substring((int)(pattern % charset.Length), 1);
            s += GenerateNumber(charset, pattern / charset.Length);

            return s;
        }
        #endregion

        #region Events
        public void OnActivated()
        {
            layoutControl.OnActivated();
        }

        public void OnDeactivated()
        {
            layoutControl.OnDeactivated();
        }

        public void OnToggleInsert(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnToggleInsert(sender, e);
        }

        private void OnDelete(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnDelete(sender, e);
        }

        private void OnBackspace(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnBackspace(sender, e);
        }

        private void OnTabForward(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnTabForward(sender, e);
        }

        private void OnEnterParagraphBreak(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.InsertNewItem(sender, e);
        }

        private void OnToggleUserVisible(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.ToggleUserVisible(sender, e);
        }

        private void OnRemoveExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.RemoveExistingItem(sender, e);
        }

        private void OnSplitExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.SplitExistingItem(sender, e);
        }

        private void OnMergeExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.MergeExistingItem(sender, e);
        }

        private void OnCycleThroughExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.CycleThroughExistingItem(sender, e);
        }

        private void OnSimplifyThroughExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.SimplifyExistingItem(sender, e);
        }

        private void OnToggleReplicate(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.ToggleReplicate(sender, e);
        }

        private void OnRedo(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Redo(sender, e);
        }

        private void OnUndo(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Undo(sender, e);
        }

        private void OnExpand(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Expand(sender, e);
        }

        private void OnReduce(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Reduce(sender, e);
        }

        private void OnExtendSelection(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.ExtendSelection(sender, e);
        }

        private void OnReduceSelection(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.ReduceSelection(sender, e);
        }

        public void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnCopy(sender, e);
        }

        public void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnCut(sender, e);
        }

        public void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnPaste(sender, e);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            layoutControl.OnMouseDown(sender, e);
        }
        #endregion

        #region Layout
        private void LoadFileLayout(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            Controller = LayoutController.Create(RootIndex);
            layoutControl.SetController(Controller);
        }

        private void UpdateView()
        {
            layoutControl.InvalidateVisual();
        }
        #endregion

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void NotifyThisPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }
}
