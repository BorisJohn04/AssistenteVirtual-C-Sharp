using System;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Net;
using System.Web;
using MySql.Data.MySqlClient;

namespace VandaAssistant
{
    public class CodigosVanda : Form1
    {
        public static object resultado = 0;
        public static Label legenda;
        public static bool runTela1;
        public static System.Timers.Timer tempo;
        public static Panel vandaFace;
        public static Button buttonInicial;
        public static PictureBox pic;
        public static Form faceVandaMacro;
        public static Form faceVandaMacro2;
        public static Form faceVandaMicro;
        public static Label totalProcessos;
        public static ListView lista;
        public static bool onMinuto;
        public static bool onSegundo;
        public static bool onHora;
        public static double horas;
        public static double minutos;
        public static double segundos;
        public static bool dispararRelogio;
        public static List<string> tasks;
        public static List<double> memoria;
        public static int count;
        public static ListView interfaceLista;
        public static Form listaProcessos;
        public static string palavraDita2;
        public static string palavraDita3;
        public static SpeechSynthesizer reconhecedor3;

        public static int calculadora(int number1, int number2)
        { 
            resultado = number1 * number2;
            legenda.Text = resultado.ToString();
            return int.Parse(resultado.ToString());
        }
        //public static void allGramatics()
        ////{
        ////    SpeechSynthesizer reconhecedor2 = new SpeechSynthesizer();
        ////    SpeechRecognitionEngine recognizer2 = new SpeechRecognitionEngine();
        ////    recognizer2.LoadGrammarAsync(new DictationGrammar());
        ////    recognizer2.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized2);//chama função de recognição
        ////    recognizer2.SetInputToDefaultAudioDevice();//seleciona o dispositivo de fala padrão usado pelo sistema
        ////    reconhecedor2.SetOutputToDefaultAudioDevice();//seleciona o dispositivo de saida
        ////    recognizer2.RecognizeAsync(RecognizeMode.Multiple);//especifica multiplos reconhecimentos, invés de apenas 1;
        ////    otherGramatic = true;

        ////}
        public static void recognizer_SpeechRecognized2(object sender, SpeechRecognizedEventArgs e2)//função de reconhecimento
        {
            palavraDita2 = e2.Result.Text;
            SendKeys.Send(e2.Result.Text);
            SendKeys.SendWait(" ");
        }

        //códigos de interface

        public static void tela1()
        {
            
            faceVandaMacro = new Form();
            faceVandaMacro.Size = new Size(424, 493);
            faceVandaMacro.FormBorderStyle = FormBorderStyle.None;
            faceVandaMacro.Visible = true;
            faceVandaMacro.Opacity = 0;
            faceVandaMacro.BackColor = Color.FromArgb(255, Color.Black);
            picStart();

            buttonInicial = new Button();
            buttonInicial.FlatStyle = FlatStyle.Flat;
            buttonInicial.Location = new Point(180, 400);
            buttonInicial.BackColor = Color.DodgerBlue;
            buttonInicial.Size = new Size(61, 25);
            buttonInicial.Click += new System.EventHandler(closeVanda);
            buttonInicial.Visible = true;
            buttonInicial.BringToFront();
            buttonInicial.Text = "Sair";
            buttonInicial.FlatAppearance.BorderColor = Color.Cyan;
            buttonInicial.FlatAppearance.BorderSize = 1;
            buttonInicial.Parent = faceVandaMacro;

            legenda = new Label();
            legenda.Font = new Font("", 13);
            legenda.Location = new Point(22, 310);
            legenda.Visible = true;
            legenda.Size = new Size(400, 400);
            legenda.Text = "";
            legenda.ForeColor = Color.White;
            legenda.Parent = faceVandaMacro;

            //tempo de fade desta tela;
            tempo = new System.Timers.Timer();
            tempo.Elapsed += CodigosVanda.fadeTelaInicial;
            tempo.AutoReset = true;
            tempo.Interval = 1;

            runTela1 = true;





        }//criador da tela inicial

        public static void fadeTelaInicial(object sender, ElapsedEventArgs e)
        {
            faceVandaMacro.Invoke(new Action(() => faceVandaMacro.Opacity += 0.06));
            if (faceVandaMacro.Opacity == 1)
            {
                tempo = new System.Timers.Timer();
                tempo.Enabled = false;
            }

        }//fade da tela inicial

        public static void picStart()
        {
            pic = new System.Windows.Forms.PictureBox();
            pic.BackgroundImage = global::VandaAssistant.Properties.Resources._60924b514b8cb43e88a99dec7356a281;
            pic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Cursor = System.Windows.Forms.Cursors.Hand;
            pic.Image = global::VandaAssistant.Properties.Resources._60924b514b8cb43e88a99dec7356a281;
            pic.Location = new System.Drawing.Point(22, 12);
            pic.Name = "pictureBox1";
            pic.Size = new System.Drawing.Size(381, 295);
            pic.TabIndex = 0;
            pic.TabStop = false;
            pic.Click += new System.EventHandler(pictureBox1_Click);
            pic.Parent = CodigosVanda.faceVandaMacro;



        }//criador da imagem de interface inicial

        public static void interfaceConstruct(){

            Form telaProcessos = new Form();
            telaProcessos.Size = new Size(250, 400);
            telaProcessos.Text = "Lista de processos ativos";
            telaProcessos.Location = new Point(0, 0);
            telaProcessos.BackColor = Color.Black;
            telaProcessos.Visible = true;
            telaProcessos.FormBorderStyle = FormBorderStyle.None;


            interfaceLista = new ListView();
            interfaceLista.Size = new Size(240, 350);
            interfaceLista.Columns.Add("Processos", 130);
            interfaceLista.Columns.Add("Memória", 100);
            interfaceLista.Parent = telaProcessos;
            interfaceLista.Visible = true;
            interfaceLista.Font = new Font("", 10);
            interfaceLista.ForeColor = Color.White;
            interfaceLista.BackColor = Color.Black;
            interfaceLista.View = View.Details;
            interfaceLista.GridLines = true;


            Label totalProcessos = new Label();
            totalProcessos.Size = new Size(30, 40);
            totalProcessos.Font = new Font("", 40);
            totalProcessos.Location = new Point(0, 0);
            totalProcessos.Visible = true;
            totalProcessos.Visible = true;
            totalProcessos.Parent = telaProcessos;


            //código de gerenciamento de processos
            string processos;
            foreach (var process in Process.GetProcesses())
            {
                count += 1;
                processos = process.ProcessName;
                interfaceLista.Invoke(new Action(() => interfaceLista.Items.Add(processos.ToString())));

                double memory = process.PrivateMemorySize64 / 1024 / 1024;
                interfaceLista.Invoke(new Action(() => interfaceLista.Items[count - 1].SubItems.Add(memory.ToString())));//acessa os subitems em tempo real, adicionando valores de acordo com o Count recebido  
            }
            totalProcessos.Text = count.ToString();
        }//criador de listas de processos

        public static void listaComandosInterface()
        {
            string names;
            Form listaForm = new Form();
            listaForm.Size = new Size(240, 400);
            listaForm.Location = new Point(0, 0);
            listaForm.BackColor = Color.Black;
            listaForm.Text = "Lista de comandos";
            listaForm.Visible = true;

            ListView list = new ListView();
            list.Size = new Size(240, 350);
            list.BackColor = Color.White;
            list.Columns.Add("Comandos", 170);
            list.GridLines = true;
            list.View = View.Details;
            list.Visible = true;
            list.Parent = listaForm;


            foreach (string i in comandos2.commands)
            {
                names = i.ToString();
                list.Items.Add(names);
            }


        }//criador da lista de comandos

        public static void relogio(object source, System.Timers.ElapsedEventArgs e) {

            if (hora.InvokeRequired) {
                hora.Invoke(new Action(() => hora.Text = DateTime.Now.ToString("HH:mm:ss")));
            }
        }//função que é chamada no temporizador do relógio
       
        public static void upgrade()
        {
            count = 0;
            string names;
            double memory;
            interfaceLista.Invoke(new Action(()=> interfaceLista.Items.Clear()));//limpa todos os processos, nomes e valores de memória
            foreach (var process in Process.GetProcesses())//busca novos processos
            {
                count += 1;
                process.Refresh();//atualiza os valores de cada processo
                interfaceLista.Items.Add(process.ProcessName.ToString());//adiciona os novos processos na lista visual

                memory = process.PrivateMemorySize64 / 1024 / 1024;
                interfaceLista.Items[count - 1].SubItems.Add(memory.ToString());

                totalProcessos = new Label();
                totalProcessos.Text = count.ToString();
            } 
        }//atualizador da lista de processos

        public static void closeVanda(object sender, EventArgs e)
        {
            foreach(var process in Process.GetProcessesByName("VandaAssistant"))
            {
                process.Kill();
            }
        }//encerrar vanda

        public static void operacoes()
        {
            if (palavraDita == "um vezes um")
            {
                calculadora(1, 1);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }

            if (palavraDita == "um vezes dois")
            {
                calculadora(1, 2);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes três")
            {
                calculadora(1, 3);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes quatro")
            {
                calculadora(1, 4);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes cinco")
            {
                calculadora(1, 5);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes seis")
            {
                calculadora(1, 6);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes sete")
            {
                calculadora(1, 7);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes oito")
            {
                calculadora(1, 8);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes nove")
            {
                calculadora(1, 9);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
            if (palavraDita == "um vezes dez")
            {
                calculadora(1, 10);
                CodigosVanda.legenda.Location = new Point(190, 310);
            }
        }


        public static void createNavigator()
        {
            Form formNavigator = new Form();
            formNavigator.Size = new Size(600, 600);
            formNavigator.Location = new Point(0, 0);
            formNavigator.BackColor = Color.Black;
            formNavigator.Visible = true;
            //formNavigator.FormBorderStyle = FormBorderStyle.None;
            formNavigator.Text = "Horus navigator";

            WebBrowser navigator = new WebBrowser();
            navigator.Size = new Size(600, 600);
            navigator.Navigate(new Uri("http://www.google.com.br//"));
            navigator.Parent = formNavigator; 
        }
    }

    public class mysqlConnection
    {
        public static void connectionData()
        {
            string email = "host@hotmail.com";
            string senha = "123";
            // let's build our link
            WebClient wc = new WebClient();
            string link = "https://vandaassistente.000webhostapp.com/API-PHP.php?email=" + email + "&senha=" + senha;
             // res is data printed by echo

            using(WebClient client = new WebClient())
            {
                string res = client.DownloadString(link);
            }
        }
    }
}