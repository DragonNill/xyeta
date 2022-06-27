using PcSborka.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace PcSborka.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для FinishToOrderWindow.xaml
    /// </summary>
    public partial class FinishToOrderWindow : Window
    {
        public CreatePcForThePeopl_dbEntities DbContext;
        public Order OrderID;
        public double FinnalyAllInCost,AllInCost = 0;

        public FinishToOrderWindow(Order order)
        {
            InitializeComponent();

            DbContext = CreatePcForThePeopl_dbEntities.DBContext;

            OrderID = order;
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (OrderID.Computer.PowerSupply == null)
            {
                Email_textBlock.Text = $"Электронная почта заказчика: {OrderID.Email}";
                Address_textBlock.Text = $"Адрес заказчика: {OrderID.Address}";
                Phone_textBlock.Text = $"Телефон заказчика: {OrderID.Phone}";

                if (OrderID.Periphery != null)
                {
                    FinishThePeriphery_button.IsEnabled = true;
                    AllInCost += Convert.ToDouble(OrderID.Periphery.SumPeriphery);
                }
            }
            else
            {
                Email_textBlock.Text = $"Электронная почта заказчика: {OrderID.Email}";
                Address_textBlock.Text = $"Адрес заказчика: {OrderID.Address}";
                Phone_textBlock.Text = $"Телефон заказчика: {OrderID.Phone}";
                FinishTheComputer_button.Content = "Посмотреть сборкy";
                AllInCost = Convert.ToDouble(OrderID.Computer.SumComponents);

                if (OrderID.Periphery != null)
                {
                    FinishThePeriphery_button.IsEnabled = true;
                    AllInCost += Convert.ToDouble(OrderID.Periphery.SumPeriphery);

                }
                EndToOrder_button.Visibility = Visibility.Visible;

                AllInCost_textBlock.Text = $"Цена сборки: {AllInCost} + ";

                FinnalyAllInCost = AllInCost;

                FinalComputerAssembly_textBlock.Text = $" = Финальная сумма: {FinnalyAllInCost} рублей.";
            }
        }

        private void FinishTheComputer_button_Click(object sender, RoutedEventArgs e)
        {
            
            Hide();
            ChooseCurrentItemShow.ComputerReadyNot = OrderID.Computer;
            ChooseWindow.isFinish = true;
            Window finishComputer = new ChooseWindow(1);
            finishComputer.ShowDialog();
            Show();
            if (OrderID.Computer.PowerSupply != null)
            {
                ChooseWindow.isFinish = false;
                ChooseCurrentItemShow.ComputerReadyNot = new Computer();
                if (OrderID.Periphery == null)
                {
                    AllInCost = Convert.ToDouble(OrderID.Computer.SumComponents);
                }
                else
                {
                    AllInCost = Convert.ToDouble(OrderID.Computer.SumComponents) + Convert.ToDouble(OrderID.Periphery.SumPeriphery); ;
                }
                EndToOrder_button.Visibility = Visibility.Visible;
                FinnalyAllInCost = AllInCost;
                AllInCost_textBlock.Text = $"Цена сборки: {AllInCost} + ";
                FinalComputerAssembly_textBlock.Text = $" = Финальная сумма: {FinnalyAllInCost} рублей.";
            }
            else
            {
                ChooseWindow.isFinish = false;
                ChooseCurrentItemShow.ComputerReadyNot = new Computer();
            }
        }

        private void FinalCost_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void FinalCost_textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void FinalCost_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FinalCost_textBox.Text))
            {
                FinnalyAllInCost = AllInCost + Convert.ToDouble(FinalCost_textBox.Text);
                FinalComputerAssembly_textBlock.Text = $" = Финальная сумма: {FinnalyAllInCost} рублей.";
            }
            else
            {
                FinnalyAllInCost = AllInCost;
                FinalComputerAssembly_textBlock.Text = $" = Финальная сумма: {FinnalyAllInCost} рублей.";
            }
        }

        private void EndToOrder_button_Click(object sender, RoutedEventArgs e)
        {
            if (OrderID.Computer.PowerSupply != null)
            {
                OrderID.Accepted = true;
                OrderID.Employeer = MenuForTheEmployeer.EmployeerID;
                DbContext.SaveChanges();

                OrderID.SumOrder = FinnalyAllInCost;

                // создание документа с начальными данными и его отображение
                Word.Application wordApp = new Word.Application();
                wordApp.Visible = true;
                Object template = Type.Missing;
                Object newTemplate = false;
                Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
                Object visible = true;
                object missing = Type.Missing;
                Word._Document wordDoc = wordApp.Documents.Add(
                    ref missing, ref missing, ref missing, ref missing);
                object start = 0, end = 0;

                // создаем диапазон, в котором будем выводить информацию
                Word.Range range = wordDoc.Range(ref start, ref end);
                range.Text = $"Заказ № {OrderID.ID}\n".ToUpper();
                // формат диапазона
                range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                range.ParagraphFormat.SpaceAfter = 0;
                range.Font.Name = "Times New Roman";

                range.Font.Size = 14;

                start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                range = wordDoc.Range(ref start, ref end);

                range.Text = $"Дата и время {OrderID.DateOrder}\n";
                range.Text += $"Компания: CreatePcForThePeople\n";

                range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                range.ParagraphFormat.SpaceAfter = 0;
                range.Font.Name = "Times New Roman";

                range.Font.Size = 14;

                start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                range = wordDoc.Range(ref start, ref end);
                // создаем таблицу
                if (OrderID.Periphery != null)
                {
                    if (OrderID.Periphery.Mouse != null && OrderID.Periphery.Keyboard != null && OrderID.Periphery.Monitor != null)
                    {
                        Word.Table table = wordDoc.Tables.Add(range, 10, 5, missing, missing);
                        table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        table.Range.Font.Name = "Times New Roman";
                        table.Cell(1, 1).Range.Text = "Артикул";
                        table.Cell(1, 2).Range.Text = "Наименование";
                        table.Cell(1, 3).Range.Text = "Цена за ед.";
                        table.Cell(1, 4).Range.Text = "Количество";
                        table.Cell(1, 5).Range.Text = "Общая цена";

                        table.Cell(1, 1).Range.Font.Size = 14;
                        table.Cell(1, 2).Range.Font.Size = 14;
                        table.Cell(1, 3).Range.Font.Size = 14;
                        table.Cell(1, 4).Range.Font.Size = 14;
                        table.Cell(1, 5).Range.Font.Size = 14;

                        //CPU
                        table.Cell(0 + 2, 1).Range.Text = OrderID.Computer.CPU.ID.ToString();
                        table.Cell(0 + 2, 1).Range.Font.Size = 14;
                        table.Cell(0 + 2, 2).Range.Text = OrderID.Computer.CPU.Name;
                        table.Cell(0 + 2, 2).Range.Font.Size = 14;
                        table.Cell(0 + 2, 3).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                        table.Cell(0 + 2, 3).Range.Font.Size = 14;
                        table.Cell(0 + 2, 4).Range.Text = "1";
                        table.Cell(0 + 2, 4).Range.Font.Size = 14;
                        table.Cell(0 + 2, 5).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                        table.Cell(0 + 2, 5).Range.Font.Size = 14;
                        //MotherBoard
                        table.Cell(1 + 2, 1).Range.Text = OrderID.Computer.MotherBoard.ID.ToString();
                        table.Cell(1 + 2, 1).Range.Font.Size = 14;
                        table.Cell(1 + 2, 2).Range.Text = OrderID.Computer.MotherBoard.Name;
                        table.Cell(1 + 2, 2).Range.Font.Size = 14;
                        table.Cell(1 + 2, 3).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                        table.Cell(1 + 2, 3).Range.Font.Size = 14;
                        table.Cell(1 + 2, 4).Range.Text = "1";
                        table.Cell(1 + 2, 4).Range.Font.Size = 14;
                        table.Cell(1 + 2, 5).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                        table.Cell(1 + 2, 5).Range.Font.Size = 14;
                        //Case
                        table.Cell(2 + 2, 1).Range.Text = OrderID.Computer.Case.ID.ToString();
                        table.Cell(2 + 2, 1).Range.Font.Size = 14;
                        table.Cell(2 + 2, 2).Range.Text = OrderID.Computer.Case.Name;
                        table.Cell(2 + 2, 2).Range.Font.Size = 14;
                        table.Cell(2 + 2, 3).Range.Text = OrderID.Computer.Case.Cost.ToString();
                        table.Cell(2 + 2, 3).Range.Font.Size = 14;
                        table.Cell(2 + 2, 4).Range.Text = "1";
                        table.Cell(2 + 2, 4).Range.Font.Size = 14;
                        table.Cell(2 + 2, 5).Range.Text = OrderID.Computer.Case.Cost.ToString();
                        table.Cell(2 + 2, 5).Range.Font.Size = 14;
                        //GPU
                        table.Cell(3 + 2, 1).Range.Text = OrderID.Computer.GPU.ID.ToString();
                        table.Cell(3 + 2, 1).Range.Font.Size = 14;
                        table.Cell(3 + 2, 2).Range.Text = OrderID.Computer.GPU.Name;
                        table.Cell(3 + 2, 2).Range.Font.Size = 14;
                        table.Cell(3 + 2, 3).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                        table.Cell(3 + 2, 3).Range.Font.Size = 14;
                        table.Cell(3 + 2, 4).Range.Text = "1";
                        table.Cell(3 + 2, 4).Range.Font.Size = 14;
                        table.Cell(3 + 2, 5).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                        table.Cell(3 + 2, 5).Range.Font.Size = 14;
                        //Cooler
                        table.Cell(4 + 2, 1).Range.Text = OrderID.Computer.Cooler.ID.ToString();
                        table.Cell(4 + 2, 1).Range.Font.Size = 14;
                        table.Cell(4 + 2, 2).Range.Text = OrderID.Computer.Cooler.Name;
                        table.Cell(4 + 2, 2).Range.Font.Size = 14;
                        table.Cell(4 + 2, 3).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                        table.Cell(4 + 2, 3).Range.Font.Size = 14;
                        table.Cell(4 + 2, 4).Range.Text = "1";
                        table.Cell(4 + 2, 4).Range.Font.Size = 14;
                        table.Cell(4 + 2, 5).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                        table.Cell(4 + 2, 5).Range.Font.Size = 14;
                        //RAM
                        table.Cell(5 + 2, 1).Range.Text = OrderID.Computer.RAM.ID.ToString();
                        table.Cell(5 + 2, 1).Range.Font.Size = 14;
                        table.Cell(5 + 2, 2).Range.Text = OrderID.Computer.RAM.Name;
                        table.Cell(5 + 2, 2).Range.Font.Size = 14;
                        table.Cell(5 + 2, 3).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                        table.Cell(5 + 2, 3).Range.Font.Size = 14;
                        table.Cell(5 + 2, 4).Range.Text = "1";
                        table.Cell(5 + 2, 4).Range.Font.Size = 14;
                        table.Cell(5 + 2, 5).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                        table.Cell(5 + 2, 5).Range.Font.Size = 14;
                        //PowerSupply
                        table.Cell(6 + 2, 1).Range.Text = OrderID.Computer.PowerSupply.ID.ToString();
                        table.Cell(6 + 2, 1).Range.Font.Size = 14;
                        table.Cell(6 + 2, 2).Range.Text = OrderID.Computer.PowerSupply.Name;
                        table.Cell(6 + 2, 2).Range.Font.Size = 14;
                        table.Cell(6 + 2, 3).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                        table.Cell(6 + 2, 3).Range.Font.Size = 14;
                        table.Cell(6 + 2, 4).Range.Text = "1";
                        table.Cell(6 + 2, 4).Range.Font.Size = 14;
                        table.Cell(6 + 2, 5).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                        table.Cell(6 + 2, 5).Range.Font.Size = 14;
                        //Monitor
                        table.Cell(7 + 2, 1).Range.Text = OrderID.Periphery.Monitor.ID.ToString();
                        table.Cell(7 + 2, 1).Range.Font.Size = 14;
                        table.Cell(7 + 2, 2).Range.Text = OrderID.Periphery.Monitor.Name;
                        table.Cell(7 + 2, 2).Range.Font.Size = 14;
                        table.Cell(7 + 2, 3).Range.Text = OrderID.Periphery.Monitor.Cost.ToString();
                        table.Cell(7 + 2, 3).Range.Font.Size = 14;
                        table.Cell(7 + 2, 4).Range.Text = "1";
                        table.Cell(7 + 2, 4).Range.Font.Size = 14;
                        table.Cell(7 + 2, 5).Range.Text = OrderID.Periphery.Monitor.Cost.ToString();
                        table.Cell(7 + 2, 5).Range.Font.Size = 14;
                        //Mouse
                        table.Cell(8 + 2, 1).Range.Text = OrderID.Periphery.Mouse.ID.ToString();
                        table.Cell(8 + 2, 1).Range.Font.Size = 14;
                        table.Cell(8 + 2, 2).Range.Text = OrderID.Periphery.Mouse.Name;
                        table.Cell(8 + 2, 2).Range.Font.Size = 14;
                        table.Cell(8 + 2, 3).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                        table.Cell(8 + 2, 3).Range.Font.Size = 14;
                        table.Cell(8 + 2, 4).Range.Text = "1";
                        table.Cell(8 + 2, 4).Range.Font.Size = 14;
                        table.Cell(8 + 2, 5).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                        table.Cell(8 + 2, 5).Range.Font.Size = 14;
                        //Keyboard
                        table.Cell(9 + 2, 1).Range.Text = OrderID.Periphery.Keyboard.ID.ToString();
                        table.Cell(9 + 2, 1).Range.Font.Size = 14;
                        table.Cell(9 + 2, 2).Range.Text = OrderID.Periphery.Keyboard.Name;
                        table.Cell(9 + 2, 2).Range.Font.Size = 14;
                        table.Cell(9 + 2, 3).Range.Text = OrderID.Periphery.Keyboard.Cost.ToString();
                        table.Cell(9 + 2, 3).Range.Font.Size = 14;
                        table.Cell(9 + 2, 4).Range.Text = "1";
                        table.Cell(9 + 2, 4).Range.Font.Size = 14;
                        table.Cell(9 + 2, 5).Range.Text = OrderID.Periphery.Keyboard.Cost.ToString();
                        table.Cell(9 + 2, 5).Range.Font.Size = 14;
                    }
                    else
                    {
                        if ((OrderID.Periphery.Mouse != null && OrderID.Periphery.Keyboard != null)
                            || (OrderID.Periphery.Mouse != null && OrderID.Periphery.Monitor != null)
                            || (OrderID.Periphery.Keyboard != null && OrderID.Periphery.Monitor != null))
                        {
                            Word.Table table = wordDoc.Tables.Add(range, 9, 5, missing, missing);
                            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            table.Range.Font.Name = "Times New Roman";
                            table.Cell(1, 1).Range.Text = "Артикул";
                            table.Cell(1, 2).Range.Text = "Наименование";
                            table.Cell(1, 3).Range.Text = "Цена за ед.";
                            table.Cell(1, 4).Range.Text = "Количество";
                            table.Cell(1, 5).Range.Text = "Общая цена";

                            table.Cell(1, 1).Range.Font.Size = 14;
                            table.Cell(1, 2).Range.Font.Size = 14;
                            table.Cell(1, 3).Range.Font.Size = 14;
                            table.Cell(1, 4).Range.Font.Size = 14;
                            table.Cell(1, 5).Range.Font.Size = 14;
                            //CPU
                            table.Cell(0 + 2, 1).Range.Text = OrderID.Computer.CPU.ID.ToString();
                            table.Cell(0 + 2, 1).Range.Font.Size = 14;
                            table.Cell(0 + 2, 2).Range.Text = OrderID.Computer.CPU.Name;
                            table.Cell(0 + 2, 2).Range.Font.Size = 14;
                            table.Cell(0 + 2, 3).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                            table.Cell(0 + 2, 3).Range.Font.Size = 14;
                            table.Cell(0 + 2, 4).Range.Text = "1";
                            table.Cell(0 + 2, 4).Range.Font.Size = 14;
                            table.Cell(0 + 2, 5).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                            table.Cell(0 + 2, 5).Range.Font.Size = 14;
                            //MotherBoard
                            table.Cell(1 + 2, 1).Range.Text = OrderID.Computer.MotherBoard.ID.ToString();
                            table.Cell(1 + 2, 1).Range.Font.Size = 14;
                            table.Cell(1 + 2, 2).Range.Text = OrderID.Computer.MotherBoard.Name;
                            table.Cell(1 + 2, 2).Range.Font.Size = 14;
                            table.Cell(1 + 2, 3).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                            table.Cell(1 + 2, 3).Range.Font.Size = 14;
                            table.Cell(1 + 2, 4).Range.Text = "1";
                            table.Cell(1 + 2, 4).Range.Font.Size = 14;
                            table.Cell(1 + 2, 5).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                            table.Cell(1 + 2, 5).Range.Font.Size = 14;
                            //Case
                            table.Cell(2 + 2, 1).Range.Text = OrderID.Computer.Case.ID.ToString();
                            table.Cell(2 + 2, 1).Range.Font.Size = 14;
                            table.Cell(2 + 2, 2).Range.Text = OrderID.Computer.Case.Name;
                            table.Cell(2 + 2, 2).Range.Font.Size = 14;
                            table.Cell(2 + 2, 3).Range.Text = OrderID.Computer.Case.Cost.ToString();
                            table.Cell(2 + 2, 3).Range.Font.Size = 14;
                            table.Cell(2 + 2, 4).Range.Text = "1";
                            table.Cell(2 + 2, 4).Range.Font.Size = 14;
                            table.Cell(2 + 2, 5).Range.Text = OrderID.Computer.Case.Cost.ToString();
                            table.Cell(2 + 2, 5).Range.Font.Size = 14;
                            //GPU
                            table.Cell(3 + 2, 1).Range.Text = OrderID.Computer.GPU.ID.ToString();
                            table.Cell(3 + 2, 1).Range.Font.Size = 14;
                            table.Cell(3 + 2, 2).Range.Text = OrderID.Computer.GPU.Name;
                            table.Cell(3 + 2, 2).Range.Font.Size = 14;
                            table.Cell(3 + 2, 3).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                            table.Cell(3 + 2, 3).Range.Font.Size = 14;
                            table.Cell(3 + 2, 4).Range.Text = "1";
                            table.Cell(3 + 2, 4).Range.Font.Size = 14;
                            table.Cell(3 + 2, 5).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                            table.Cell(3 + 2, 5).Range.Font.Size = 14;
                            //Cooler
                            table.Cell(4 + 2, 1).Range.Text = OrderID.Computer.Cooler.ID.ToString();
                            table.Cell(4 + 2, 1).Range.Font.Size = 14;
                            table.Cell(4 + 2, 2).Range.Text = OrderID.Computer.Cooler.Name;
                            table.Cell(4 + 2, 2).Range.Font.Size = 14;
                            table.Cell(4 + 2, 3).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                            table.Cell(4 + 2, 3).Range.Font.Size = 14;
                            table.Cell(4 + 2, 4).Range.Text = "1";
                            table.Cell(4 + 2, 4).Range.Font.Size = 14;
                            table.Cell(4 + 2, 5).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                            table.Cell(4 + 2, 5).Range.Font.Size = 14;
                            //RAM
                            table.Cell(5 + 2, 1).Range.Text = OrderID.Computer.RAM.ID.ToString();
                            table.Cell(5 + 2, 1).Range.Font.Size = 14;
                            table.Cell(5 + 2, 2).Range.Text = OrderID.Computer.RAM.Name;
                            table.Cell(5 + 2, 2).Range.Font.Size = 14;
                            table.Cell(5 + 2, 3).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                            table.Cell(5 + 2, 3).Range.Font.Size = 14;
                            table.Cell(5 + 2, 4).Range.Text = "1";
                            table.Cell(5 + 2, 4).Range.Font.Size = 14;
                            table.Cell(5 + 2, 5).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                            table.Cell(5 + 2, 5).Range.Font.Size = 14;
                            //PowerSupply
                            table.Cell(6 + 2, 1).Range.Text = OrderID.Computer.PowerSupply.ID.ToString();
                            table.Cell(6 + 2, 1).Range.Font.Size = 14;
                            table.Cell(6 + 2, 2).Range.Text = OrderID.Computer.PowerSupply.Name;
                            table.Cell(6 + 2, 2).Range.Font.Size = 14;
                            table.Cell(6 + 2, 3).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                            table.Cell(6 + 2, 3).Range.Font.Size = 14;
                            table.Cell(6 + 2, 4).Range.Text = "1";
                            table.Cell(6 + 2, 4).Range.Font.Size = 14;
                            table.Cell(6 + 2, 5).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                            table.Cell(6 + 2, 5).Range.Font.Size = 14;
                            if (OrderID.Periphery.Monitor != null)
                            {
                                //Monitor
                                table.Cell(7 + 2, 1).Range.Text = OrderID.Periphery.Monitor.ID.ToString();
                                table.Cell(7 + 2, 1).Range.Font.Size = 14;
                                table.Cell(7 + 2, 2).Range.Text = OrderID.Periphery.Monitor.Name;
                                table.Cell(7 + 2, 2).Range.Font.Size = 14;
                                table.Cell(7 + 2, 3).Range.Text = OrderID.Periphery.Monitor.Cost.ToString();
                                table.Cell(7 + 2, 3).Range.Font.Size = 14;
                                table.Cell(7 + 2, 4).Range.Text = "1";
                                table.Cell(7 + 2, 4).Range.Font.Size = 14;
                                table.Cell(7 + 2, 5).Range.Text = OrderID.Periphery.Monitor.Cost.ToString();
                                table.Cell(7 + 2, 5).Range.Font.Size = 14;
                            }
                            if (OrderID.Periphery.Mouse != null)
                            {
                                if (OrderID.Periphery.Monitor != null)
                                {
                                    //Mouse
                                    table.Cell(8 + 2, 1).Range.Text = OrderID.Periphery.Mouse.ID.ToString();
                                    table.Cell(8 + 2, 1).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 2).Range.Text = OrderID.Periphery.Mouse.Name;
                                    table.Cell(8 + 2, 2).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 3).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                                    table.Cell(8 + 2, 3).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 4).Range.Text = "1";
                                    table.Cell(8 + 2, 4).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 5).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                                    table.Cell(8 + 2, 5).Range.Font.Size = 14;
                                }
                                else
                                {
                                    table.Cell(7 + 2, 1).Range.Text = OrderID.Periphery.Mouse.ID.ToString();
                                    table.Cell(7 + 2, 1).Range.Font.Size = 14;
                                    table.Cell(7 + 2, 2).Range.Text = OrderID.Periphery.Mouse.Name;
                                    table.Cell(7 + 2, 2).Range.Font.Size = 14;
                                    table.Cell(7 + 2, 3).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                                    table.Cell(7 + 2, 3).Range.Font.Size = 14;
                                    table.Cell(7 + 2, 4).Range.Text = "1";
                                    table.Cell(7 + 2, 4).Range.Font.Size = 14;
                                    table.Cell(7 + 2, 5).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                                    table.Cell(7 + 2, 5).Range.Font.Size = 14;
                                }
                            }
                            if (OrderID.Periphery.Keyboard != null)
                            {
                                if (OrderID.Periphery.Monitor != null || OrderID.Periphery.Mouse != null)
                                {
                                    //Keyboard
                                    table.Cell(8 + 2, 1).Range.Text = OrderID.Periphery.Keyboard.ID.ToString();
                                    table.Cell(8 + 2, 1).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 2).Range.Text = OrderID.Periphery.Keyboard.Name;
                                    table.Cell(8 + 2, 2).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 3).Range.Text = OrderID.Periphery.Keyboard.Cost.ToString();
                                    table.Cell(8 + 2, 3).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 4).Range.Text = "1";
                                    table.Cell(8 + 2, 4).Range.Font.Size = 14;
                                    table.Cell(8 + 2, 5).Range.Text = OrderID.Periphery.Keyboard.Cost.ToString();
                                    table.Cell(8 + 2, 5).Range.Font.Size = 14;
                                }
                            }
                        }
                        else
                        {
                            Word.Table table = wordDoc.Tables.Add(range, 8, 5, missing, missing);
                            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            table.Range.Font.Name = "Times New Roman";
                            table.Cell(1, 1).Range.Text = "Артикул";
                            table.Cell(1, 2).Range.Text = "Наименование";
                            table.Cell(1, 3).Range.Text = "Цена за ед.";
                            table.Cell(1, 4).Range.Text = "Количество";
                            table.Cell(1, 5).Range.Text = "Общая цена";

                            table.Cell(1, 1).Range.Font.Size = 14;
                            table.Cell(1, 2).Range.Font.Size = 14;
                            table.Cell(1, 3).Range.Font.Size = 14;
                            table.Cell(1, 4).Range.Font.Size = 14;
                            table.Cell(1, 5).Range.Font.Size = 14;
                            //CPU
                            table.Cell(0 + 2, 1).Range.Text = OrderID.Computer.CPU.ID.ToString();
                            table.Cell(0 + 2, 1).Range.Font.Size = 14;
                            table.Cell(0 + 2, 2).Range.Text = OrderID.Computer.CPU.Name;
                            table.Cell(0 + 2, 2).Range.Font.Size = 14;
                            table.Cell(0 + 2, 3).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                            table.Cell(0 + 2, 3).Range.Font.Size = 14;
                            table.Cell(0 + 2, 4).Range.Text = "1";
                            table.Cell(0 + 2, 4).Range.Font.Size = 14;
                            table.Cell(0 + 2, 5).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                            table.Cell(0 + 2, 5).Range.Font.Size = 14;
                            //MotherBoard
                            table.Cell(1 + 2, 1).Range.Text = OrderID.Computer.MotherBoard.ID.ToString();
                            table.Cell(1 + 2, 1).Range.Font.Size = 14;
                            table.Cell(1 + 2, 2).Range.Text = OrderID.Computer.MotherBoard.Name;
                            table.Cell(1 + 2, 2).Range.Font.Size = 14;
                            table.Cell(1 + 2, 3).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                            table.Cell(1 + 2, 3).Range.Font.Size = 14;
                            table.Cell(1 + 2, 4).Range.Text = "1";
                            table.Cell(1 + 2, 4).Range.Font.Size = 14;
                            table.Cell(1 + 2, 5).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                            table.Cell(1 + 2, 5).Range.Font.Size = 14;
                            //Case
                            table.Cell(2 + 2, 1).Range.Text = OrderID.Computer.Case.ID.ToString();
                            table.Cell(2 + 2, 1).Range.Font.Size = 14;
                            table.Cell(2 + 2, 2).Range.Text = OrderID.Computer.Case.Name;
                            table.Cell(2 + 2, 2).Range.Font.Size = 14;
                            table.Cell(2 + 2, 3).Range.Text = OrderID.Computer.Case.Cost.ToString();
                            table.Cell(2 + 2, 3).Range.Font.Size = 14;
                            table.Cell(2 + 2, 4).Range.Text = "1";
                            table.Cell(2 + 2, 4).Range.Font.Size = 14;
                            table.Cell(2 + 2, 5).Range.Text = OrderID.Computer.Case.Cost.ToString();
                            table.Cell(2 + 2, 5).Range.Font.Size = 14;
                            //GPU
                            table.Cell(3 + 2, 1).Range.Text = OrderID.Computer.GPU.ID.ToString();
                            table.Cell(3 + 2, 1).Range.Font.Size = 14;
                            table.Cell(3 + 2, 2).Range.Text = OrderID.Computer.GPU.Name;
                            table.Cell(3 + 2, 2).Range.Font.Size = 14;
                            table.Cell(3 + 2, 3).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                            table.Cell(3 + 2, 3).Range.Font.Size = 14;
                            table.Cell(3 + 2, 4).Range.Text = "1";
                            table.Cell(3 + 2, 4).Range.Font.Size = 14;
                            table.Cell(3 + 2, 5).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                            table.Cell(3 + 2, 5).Range.Font.Size = 14;
                            //Cooler
                            table.Cell(4 + 2, 1).Range.Text = OrderID.Computer.Cooler.ID.ToString();
                            table.Cell(4 + 2, 1).Range.Font.Size = 14;
                            table.Cell(4 + 2, 2).Range.Text = OrderID.Computer.Cooler.Name;
                            table.Cell(4 + 2, 2).Range.Font.Size = 14;
                            table.Cell(4 + 2, 3).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                            table.Cell(4 + 2, 3).Range.Font.Size = 14;
                            table.Cell(4 + 2, 4).Range.Text = "1";
                            table.Cell(4 + 2, 4).Range.Font.Size = 14;
                            table.Cell(4 + 2, 5).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                            table.Cell(4 + 2, 5).Range.Font.Size = 14;
                            //RAM
                            table.Cell(5 + 2, 1).Range.Text = OrderID.Computer.RAM.ID.ToString();
                            table.Cell(5 + 2, 1).Range.Font.Size = 14;
                            table.Cell(5 + 2, 2).Range.Text = OrderID.Computer.RAM.Name;
                            table.Cell(5 + 2, 2).Range.Font.Size = 14;
                            table.Cell(5 + 2, 3).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                            table.Cell(5 + 2, 3).Range.Font.Size = 14;
                            table.Cell(5 + 2, 4).Range.Text = "1";
                            table.Cell(5 + 2, 4).Range.Font.Size = 14;
                            table.Cell(5 + 2, 5).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                            table.Cell(5 + 2, 5).Range.Font.Size = 14;
                            //PowerSupply
                            table.Cell(6 + 2, 1).Range.Text = OrderID.Computer.PowerSupply.ID.ToString();
                            table.Cell(6 + 2, 1).Range.Font.Size = 14;
                            table.Cell(6 + 2, 2).Range.Text = OrderID.Computer.PowerSupply.Name;
                            table.Cell(6 + 2, 2).Range.Font.Size = 14;
                            table.Cell(6 + 2, 3).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                            table.Cell(6 + 2, 3).Range.Font.Size = 14;
                            table.Cell(6 + 2, 4).Range.Text = "1";
                            table.Cell(6 + 2, 4).Range.Font.Size = 14;
                            table.Cell(6 + 2, 5).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                            table.Cell(6 + 2, 5).Range.Font.Size = 14;
                            if (OrderID.Periphery.Monitor != null)
                            {
                                //Monitor
                                table.Cell(7 + 2, 1).Range.Text = OrderID.Periphery.Monitor.ID.ToString();
                                table.Cell(7 + 2, 1).Range.Font.Size = 14;
                                table.Cell(7 + 2, 2).Range.Text = OrderID.Periphery.Monitor.Name;
                                table.Cell(7 + 2, 2).Range.Font.Size = 14;
                                table.Cell(7 + 2, 3).Range.Text = OrderID.Periphery.Monitor.Cost.ToString();
                                table.Cell(7 + 2, 3).Range.Font.Size = 14;
                                table.Cell(7 + 2, 4).Range.Text = "1";
                                table.Cell(7 + 2, 4).Range.Font.Size = 14;
                                table.Cell(7 + 2, 5).Range.Text = OrderID.Periphery.Monitor.Cost.ToString();
                                table.Cell(7 + 2, 5).Range.Font.Size = 14;
                            }
                            if (OrderID.Periphery.Mouse != null)
                            {
                                //Mouse
                                table.Cell(7 + 2, 1).Range.Text = OrderID.Periphery.Mouse.ID.ToString();
                                table.Cell(7 + 2, 1).Range.Font.Size = 14;
                                table.Cell(7 + 2, 2).Range.Text = OrderID.Periphery.Mouse.Name;
                                table.Cell(7 + 2, 2).Range.Font.Size = 14;
                                table.Cell(7 + 2, 3).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                                table.Cell(7 + 2, 3).Range.Font.Size = 14;
                                table.Cell(7 + 2, 4).Range.Text = "1";
                                table.Cell(7 + 2, 4).Range.Font.Size = 14;
                                table.Cell(7 + 2, 5).Range.Text = OrderID.Periphery.Mouse.Cost.ToString();
                                table.Cell(7 + 2, 5).Range.Font.Size = 14;
                            }
                            if (OrderID.Periphery.Keyboard != null)
                            {
                                //Keyboard
                                table.Cell(7 + 2, 1).Range.Text = OrderID.Periphery.Keyboard.ID.ToString();
                                table.Cell(7 + 2, 1).Range.Font.Size = 14;
                                table.Cell(7 + 2, 2).Range.Text = OrderID.Periphery.Keyboard.Name;
                                table.Cell(7 + 2, 2).Range.Font.Size = 14;
                                table.Cell(7 + 2, 3).Range.Text = OrderID.Periphery.Keyboard.Cost.ToString();
                                table.Cell(7 + 2, 3).Range.Font.Size = 14;
                                table.Cell(7 + 2, 4).Range.Text = "1";
                                table.Cell(7 + 2, 4).Range.Font.Size = 14;
                                table.Cell(7 + 2, 5).Range.Text = OrderID.Periphery.Keyboard.Cost.ToString();
                                table.Cell(7 + 2, 5).Range.Font.Size = 14;
                            }

                        }
                    }
                }
                else
                {
                    Word.Table table = wordDoc.Tables.Add(range, 7, 5, missing, missing);
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Name = "Times New Roman";
                    table.Cell(1, 1).Range.Text = "Артикул";
                    table.Cell(1, 2).Range.Text = "Наименование";
                    table.Cell(1, 3).Range.Text = "Цена за ед.";
                    table.Cell(1, 4).Range.Text = "Количество";
                    table.Cell(1, 5).Range.Text = "Общая цена";

                    table.Cell(1, 1).Range.Font.Size = 14;
                    table.Cell(1, 2).Range.Font.Size = 14;
                    table.Cell(1, 3).Range.Font.Size = 14;
                    table.Cell(1, 4).Range.Font.Size = 14;
                    table.Cell(1, 5).Range.Font.Size = 14;
                    //CPU
                    table.Cell(0 + 2, 1).Range.Text = OrderID.Computer.CPU.ID.ToString();
                    table.Cell(0 + 2, 1).Range.Font.Size = 14;
                    table.Cell(0 + 2, 2).Range.Text = OrderID.Computer.CPU.Name;
                    table.Cell(0 + 2, 2).Range.Font.Size = 14;
                    table.Cell(0 + 2, 3).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                    table.Cell(0 + 2, 3).Range.Font.Size = 14;
                    table.Cell(0 + 2, 4).Range.Text = "1";
                    table.Cell(0 + 2, 4).Range.Font.Size = 14;
                    table.Cell(0 + 2, 5).Range.Text = OrderID.Computer.CPU.Cost.ToString();
                    table.Cell(0 + 2, 5).Range.Font.Size = 14;
                    //MotherBoard
                    table.Cell(1 + 2, 1).Range.Text = OrderID.Computer.MotherBoard.ID.ToString();
                    table.Cell(1 + 2, 1).Range.Font.Size = 14;
                    table.Cell(1 + 2, 2).Range.Text = OrderID.Computer.MotherBoard.Name;
                    table.Cell(1 + 2, 2).Range.Font.Size = 14;
                    table.Cell(1 + 2, 3).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                    table.Cell(1 + 2, 3).Range.Font.Size = 14;
                    table.Cell(1 + 2, 4).Range.Text = "1";
                    table.Cell(1 + 2, 4).Range.Font.Size = 14;
                    table.Cell(1 + 2, 5).Range.Text = OrderID.Computer.MotherBoard.Cost.ToString();
                    table.Cell(1 + 2, 5).Range.Font.Size = 14;
                    //Case
                    table.Cell(2 + 2, 1).Range.Text = OrderID.Computer.Case.ID.ToString();
                    table.Cell(2 + 2, 1).Range.Font.Size = 14;
                    table.Cell(2 + 2, 2).Range.Text = OrderID.Computer.Case.Name;
                    table.Cell(2 + 2, 2).Range.Font.Size = 14;
                    table.Cell(2 + 2, 3).Range.Text = OrderID.Computer.Case.Cost.ToString();
                    table.Cell(2 + 2, 3).Range.Font.Size = 14;
                    table.Cell(2 + 2, 4).Range.Text = "1";
                    table.Cell(2 + 2, 4).Range.Font.Size = 14;
                    table.Cell(2 + 2, 5).Range.Text = OrderID.Computer.Case.Cost.ToString();
                    table.Cell(2 + 2, 5).Range.Font.Size = 14;
                    //GPU
                    table.Cell(3 + 2, 1).Range.Text = OrderID.Computer.GPU.ID.ToString();
                    table.Cell(3 + 2, 1).Range.Font.Size = 14;
                    table.Cell(3 + 2, 2).Range.Text = OrderID.Computer.GPU.Name;
                    table.Cell(3 + 2, 2).Range.Font.Size = 14;
                    table.Cell(3 + 2, 3).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                    table.Cell(3 + 2, 3).Range.Font.Size = 14;
                    table.Cell(3 + 2, 4).Range.Text = "1";
                    table.Cell(3 + 2, 4).Range.Font.Size = 14;
                    table.Cell(3 + 2, 5).Range.Text = OrderID.Computer.GPU.Cost.ToString();
                    table.Cell(3 + 2, 5).Range.Font.Size = 14;
                    //Cooler
                    table.Cell(4 + 2, 1).Range.Text = OrderID.Computer.Cooler.ID.ToString();
                    table.Cell(4 + 2, 1).Range.Font.Size = 14;
                    table.Cell(4 + 2, 2).Range.Text = OrderID.Computer.Cooler.Name;
                    table.Cell(4 + 2, 2).Range.Font.Size = 14;
                    table.Cell(4 + 2, 3).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                    table.Cell(4 + 2, 3).Range.Font.Size = 14;
                    table.Cell(4 + 2, 4).Range.Text = "1";
                    table.Cell(4 + 2, 4).Range.Font.Size = 14;
                    table.Cell(4 + 2, 5).Range.Text = OrderID.Computer.Cooler.Cost.ToString();
                    table.Cell(4 + 2, 5).Range.Font.Size = 14;
                    //RAM
                    table.Cell(5 + 2, 1).Range.Text = OrderID.Computer.RAM.ID.ToString();
                    table.Cell(5 + 2, 1).Range.Font.Size = 14;
                    table.Cell(5 + 2, 2).Range.Text = OrderID.Computer.RAM.Name;
                    table.Cell(5 + 2, 2).Range.Font.Size = 14;
                    table.Cell(5 + 2, 3).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                    table.Cell(5 + 2, 3).Range.Font.Size = 14;
                    table.Cell(5 + 2, 4).Range.Text = "1";
                    table.Cell(5 + 2, 4).Range.Font.Size = 14;
                    table.Cell(5 + 2, 5).Range.Text = OrderID.Computer.RAM.Cost.ToString();
                    table.Cell(5 + 2, 5).Range.Font.Size = 14;
                    //PowerSupply
                    table.Cell(6 + 2, 1).Range.Text = OrderID.Computer.PowerSupply.ID.ToString();
                    table.Cell(6 + 2, 1).Range.Font.Size = 14;
                    table.Cell(6 + 2, 2).Range.Text = OrderID.Computer.PowerSupply.Name;
                    table.Cell(6 + 2, 2).Range.Font.Size = 14;
                    table.Cell(6 + 2, 3).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                    table.Cell(6 + 2, 3).Range.Font.Size = 14;
                    table.Cell(6 + 2, 4).Range.Text = "1";
                    table.Cell(6 + 2, 4).Range.Font.Size = 14;
                    table.Cell(6 + 2, 5).Range.Text = OrderID.Computer.PowerSupply.Cost.ToString();
                    table.Cell(6 + 2, 5).Range.Font.Size = 14;
                }

                start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                range = wordDoc.Range(ref start, ref end);
                range.Text = $"\nОбщая стоимость: {OrderID.SumOrder} рублей.\n";
                range.Text += $"Сдал: {OrderID.Employeer.Name} {OrderID.Employeer.SecondName} {OrderID.Employeer.LastName}\n";
                range.Text += $"Принял: {OrderID.Employeer.Name} {OrderID.Employeer.SecondName} {OrderID.Employeer.LastName}\n";
                range.Text += $"Адрес: {OrderID.Address}";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 14;
            }
            else
            {
                MessageBox.Show("Пожалуйста доделайте заказ","Предупрежедние",MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void FinishThePeriphery_button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            ChooseCurrentItemShow.PeripheryReadyNot = OrderID.Periphery;
            ChoosePeripheryWindow.isFinish = true;
            Window finishPeriphery = new ChoosePeripheryWindow(1);
            finishPeriphery.ShowDialog();
            Show();
            ChoosePeripheryWindow.isFinish = false;
            ChooseCurrentItemShow.PeripheryReadyNot = new Periphery();
        }
    }
}
