using MvvmCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace MvvmCalculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorModel _model = new CalculatorModel();

        private string _display = "0";
        private double _firstNumber = 0;
        private string _operator = "";
        private bool _isNewInput = true;

        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(); }
        }

        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand DotCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand SqrtCommand { get; }
        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand(param => AddNumber(param.ToString()));
            OperatorCommand = new RelayCommand(param => SetOperator(param.ToString()));
            EqualCommand = new RelayCommand(param => Calculate());
            ClearCommand = new RelayCommand(param => Clear());
            BackspaceCommand = new RelayCommand(param => Backspace());
            DotCommand = new RelayCommand(param => AddDot());
            PercentCommand = new RelayCommand(param => ApplyPercent());
            SqrtCommand = new RelayCommand(param => ApplySqrt());
        }

        private void AddNumber(string num)
        {
            if (_isNewInput)
            {
                Display = num;
                _isNewInput = false;
            }
            else
            {
                Display += num;
            }
        }

        private void SetOperator(string op)
        {
            _firstNumber = double.Parse(Display);
            _operator = op;
            _isNewInput = true;
        }

         private void Calculate()
         {
              double second = double.Parse(Display);
              double result = _model.Calculate(_firstNumber, second, _operator);
              Display = result.ToString();
              _isNewInput = true;
         }
        

       
        private void Clear()
        {
            Display = "0";
            _firstNumber = 0;
            _operator = "";
            _isNewInput = true;
        }
        private void Backspace()
        {
            if (Display.Length > 1)
            {
                Display = Display.Substring(0, Display.Length - 1);
            }
            else
            {
                Display = "0";   // 刪到只剩一位時顯示回 0
            }
        }

        private void AddDot()
        {
            // 若是新的輸入（按完運算符號剛開始輸入第二個數）
            if (_isNewInput)
            {
                Display = "0.";
                _isNewInput = false;
                return;
            }

            // 若已經有小數點，不再加入
            if (Display.Contains("."))
                return;

            Display += ".";
        }

        private void AddLeftParen()
        {
            if (_isNewInput || Display == "0")
                Display = "(";
            else
                Display += "(";

            _isNewInput = false;
        }

        private void AddRightParen()
        {
            Display += ")";
            _isNewInput = false;
        }

        private void ApplyPercent()
        {
            if (string.IsNullOrWhiteSpace(Display))
                return;

            // 把最後一段數字找出來，例如 50 + 10 → 找出 "10"
            int i = Display.Length - 1;

            while (i >= 0 && (char.IsDigit(Display[i]) || Display[i] == '.'))
                i--;

            string numberPart = Display.Substring(i + 1);

            if (!double.TryParse(numberPart, out double num))
                return;

            double percentValue = num / 100.0;

            // 重新組合算式：
            Display = Display.Substring(0, i + 1) + percentValue.ToString();

            _isNewInput = false;
        }

        private void ApplySqrt()
        {
            if (string.IsNullOrWhiteSpace(Display))
                return;

            try
            {
                double number = double.Parse(Display);
                if (number < 0)
                {
                    Display = "Error"; // 負數不能開根號
                }
                else
                {
                    Display = Math.Sqrt(number).ToString();
                    _isNewInput = true;
                }
            }
            catch
            {
                Display = "Error";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
