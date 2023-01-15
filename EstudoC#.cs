using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Globalization;//informação de idioma de entrada
using System.Timers;
using System.ComponentModel;
public class Class1
{

	//ideias
	//1 - AutoWrite
	//2 - TalkBack
	public Class1()
	{

		public static Main(string[] args)
		{

		}



	}
	public class temporizador{

	public System.Timers.Timer tempo;//variável de tempo

	public void temporizadores()
	{
		tempo = new System.Timers.Timer();//cria uma nova instância de tempo
		tempo.Elapsed = onTime;//iguala a função que eu quero que seja executada constantemente dentro desse tempo
		tempo.AutoReset = true; //habilita o loop de tempo
		tempo.Enabled = true;//inicia o temporizador				
	}

	public void onTime(Object source ElapsedEventHandler e){//função que será executada no temporizador
		int result = 0;
		int i = 1;
		int b = 1;
		result = i + b;
		}
	}

	public class processos{
		//neste ponto vamos especificar um processo na lista de tasks e salvar o nome dele em uma lista e encerra-lo
		string name;
		string name2;
		List<string> taskList = new List<string>();
		public void searchProcess()
        {
			foreach(var process in Process.GetProcessesByName("discord"))//veririca especificamente a lista de processos em busca do discord
            {
				name = process.ProcessName;//armazena o nome do processo na variável 
				tasktList.Add(name);
				process.Kill();//encerra o processo
            }
			//caso queira exibir o conteúdo da lista no console
			foreach (double z in tasktList)
			{
				Console.WriteLine(z);
			}
		}
		//neste ponto vamos verificar a memória privada usada por cada processo, armazenar em lista e exibir em console
		public void memoryCheck()
        {
			List<double> memoryValue = new List<double>();
			double memory = 0;
			foreach(var process2 in Process.GetProcesses())
            {
				memory = process2.PrivateMemorySize64 / 1024 / 1024;
				//caso queira exibir no console
				Console.WriteLine(memory);
				//caso queira armazenar em uma lista
				memoryValue.Add(memory);
            }
			//caso queira exibir o conteúdo da lista no console
			foreach(double i in memoryValue)
            {
				Console.WriteLine(i);
            }
        }
		//neste ponto vamos aprender a adicionar os processo em uma lista padrão e uma lista com interface
		public void viewList()
		{
			string name2;
			Form telaProcessos = new Form();
			telaProcessos.Size = new Size(270, 400);
			telaProcessos.Text = "Lista de processos ativos";
			telaProcessos.Location = new Point(0, 0);
			telaProcessos.BackColor = Color.Black;
			telaProcessos.Visible = true;

			numberMemory = new Label();
			numberMemory.Size = new Size(20, 40);
			numberMemory.Font = new Font("arial", 40);
			numberMemory.Visible = true;
			numberMemory.Location = new Point(0, 0);
			numberMemory.Parent = telaProcessos;

			//front end da lista
			lista = new ListView();
			List<string> lista2 = new List<string>();//caso você queira atualizar a lista com interface
			List<double> memoryList = new List<double>();//caso você queira atualizar a lista com interface
			this.Controls.Add(lista);
			lista.Columns.Add("Processos", 140);//adiciona a coluna de processos
			lista.Columns.Add("Memórias", 100);//adiciona a coluna de memória
			lista.Size = new Size(400, 400);
			lista.GridLines = true;//ativa a grade de linhas da lista
			lista.View = View.Details;//exibe os detalhes da lista
			lista.Visible = true;//a torna visível
			lista.Parent = telaProcessos;


			foreach (var process3 in Process.GetProcesses())
			{
				count += 1;//conta mais um processo achado
				numberMemory.Text = count.ToString();
				name2 = process3.ProcessName;//salva ele
				lista.Items.Add(name2.ToString());//adiciona ele em uma lista do tipo double
												  //adicionando na lista com interface
				double memoriaValor = 0;
				memoriaValor = process3.PrivateMemorySize64 / 1024 / 1024;

				for (int i = 0; i < count; i++)
				{
					lista.Items[i].SubItems.Add(memoriaValor.ToString());
				}
			}
		}
		
		public void upgrade()//limpa a lista e atualiza os valores de memória, pode ser chamado por função de tempo
		{
			count = 0;
			string names;
			double memory;
			lista.Items.Clear();//limpa todos os processos, nomes e valores de memória
			foreach (var process in Process.GetProcesses())//busca novos processos
			{
				count += 1;
				names = process.ProcessName;
				lista.Items.Add(names.ToString());

				memory = process.PrivateMemorySize64 / 1024 / 1024;
				lista.Items[count - 1].SubItems.Add(memory.ToString());
				numberMemory = new Label();//para referenciar o objeto novamente
				numberMemory.Text = count.ToString();
			}
		}

		void time()
		{
			int[] targetColor = { 255, 255, 255 };
			int[] fadeRGB = new int[3];

			fadeRGB[0] = hora.ForeColor.R;
			fadeRGB[1] = hora.ForeColor.G;
			fadeRGB[2] = hora.ForeColor.B;
			//////////////////////////////
			if (fadeRGB[0] > targetColor[0])
			{
				fadeRGB[0]--;

			}
			else if (fadeRGB[0] < targetColor[0])
			{
				fadeRGB[0]++;
			}
			///////////////////////////////
			if (fadeRGB[1] > targetColor[1])
			{
				fadeRGB[1]--;
			}
			else if (fadeRGB[1] < targetColor[1])
			{
				fadeRGB[1]++;
			}
			///////////////////////////////
			if (fadeRGB[2] > targetColor[2])
			{
				fadeRGB[2]--;
			}
			else if (fadeRGB[2] < targetColor[2])
			{
				fadeRGB[2]++;
			}

			if (fadeRGB[0] == targetColor[0] && fadeRGB[1] == targetColor[1] && fadeRGB[2] == targetColor[2])
			{
				timer2.Stop();
			}

			hora.ForeColor = Color.FromArgb(CodigosVanda.faceVandaMacro.BackColor.R, CodigosVanda.faceVandaMacro.BackColor.G, CodigosVanda.faceVandaMacro.BackColor.B);
		}

		private void timer2_Tick(object sender, EventArgs e)//sequencias de mensagens na tela com temporizador
		{
			this.Hide();
			mensagem1();//contabiliza repetidas vezes

			if (i == 1)//e condiciona as mensagens
			{
				timer2.Enabled = true;
				legendaText = "Olá, eu sou a Vanda, " +
					  "é um prazer te conhecer!";
				reconhecedor.SpeakAsync("Olá, eu sou a Vanda, é um prazer te conhecer!");

			}
			else if (i == 5)//quando for igual a 5 o programa exibe o outra mensagem
			{
				legendaText = "Tudo bem?";
				reconhecedor.SpeakAsync("Tudo bêim?");
			}
			// i += 1;
			CodigosVanda.legenda.Text = legendaText.ToString();//exibe a mensagem na tela
		}

		public void mensagem1()//contabiliza
		{
			i += 1;
		}

		public void pro(object sender, ElapsedEventArgs e)//gerencimanento mais preciso do uso geral da memória física com temporizador
		{
			foreach (var process in Process.GetProcesses())//busca todos os processos
			{
				//converter bytes em MB, basta dividir o numero de bytes por 1024 duas vezes, isso serve para todas as outras unidades, gigas, megas.
				PC = new PerformanceCounter();//instancia um novo objeto para medir a performance
				PC.CategoryName = "Process";//indica a classe para checagem
				PC.CounterName = "Working Set - Private";//que tipo de valor 
				PC.InstanceName = process.ProcessName;//herda os nomes encontrados em GetProcesses para consultar a memória usada por cada um deles
				size = Convert.ToInt32(PC.NextValue()) / (int)(1024);// e converter em megabytes
				size2 += size;//e depois somar, como ele vai fazer isso para todos processos na lista, então essa soma será feita durante a contagem dos processos
				PC.Close();
				PC.Dispose();
				tempo.Stop();
			}
			result = size2 / 100;
			result2 = result / 100;
			result3 = result2 / 10;

			legendaText = Math.Round(result3).ToString() + "%".ToString(); //math.round formata o numero para reduzi-lo
		}

		public Form1()// onde a função pro será chamada
		{
			tempo = new System.Timers.Timer();
			tempo.Elapsed += pro;
			tempo.Interval = 3000;
			tempo.AutoReset = true;
			tempo.Start();
		}

		public RequisicaoWeb()//requisitando dados de um arquivo JSON via HTTP
        {
			var requisicaoWeb = WebRequest.CreateHttp("http://jsonplaceholder.typicode.com/posts/1");
			requisicaoWeb.Method = "GET";
			requisicaoWeb.UserAgent = "RequisicaoWebDemo";
			using (var resposta = requisicaoWeb.GetResponse())
			{
				var streamDados = resposta.GetResponseStream();
				StreamReader reader = new StreamReader(streamDados);
				object objResponse = reader.ReadToEnd();
				var post = JsonConvert.DeserializeObject<Post>(objResponse.ToString());
				Console.WriteLine(post.Id + " " + post.title + " " + post.body);
				Console.ReadLine();
				streamDados.Close();
				resposta.Close();
			}
			Console.ReadLine();//necessário importar as bibliotecas
							   //using System;
							   //using System.Net;
							   //using System.IO;
								//using Newtonsoft.Json;
		}

		public static void EnviaRequisicaoPOST()
		{
			string dadosPOST = "title=macoratti";
			dadosPOST = dadosPOST + "&body=teste de envio de post";
			dadosPOST = dadosPOST + "&userId=1";
			var dados = Encoding.UTF8.GetBytes(dadosPOST);
			var requisicaoWeb = WebRequest.CreateHttp("http://jsonplaceholder.typicode.com/posts");
			requisicaoWeb.Method = "POST";
			requisicaoWeb.ContentType = "application/x-www-form-urlencoded";
			requisicaoWeb.ContentLength = dados.Length;
			requisicaoWeb.UserAgent = "RequisicaoWebDemo";
			//precisamos escrever os dados post para o stream
			using (var stream = requisicaoWeb.GetRequestStream())
			{
				stream.Write(dados, 0, dados.Length);
				stream.Close();
			}
			//ler e exibir a resposta
			using (var resposta = requisicaoWeb.GetResponse())
			{
				var streamDados = resposta.GetResponseStream();
				StreamReader reader = new StreamReader(streamDados);
				object objResponse = reader.ReadToEnd();
				var post = JsonConvert.DeserializeObject<Post>(objResponse.ToString());
				Console.WriteLine(post.Id + " " + post.title + " " + post.body);
				streamDados.Close();
				resposta.Close();
			}
			Console.ReadLine();
		}//alterando dado de JSON com HTTP 

		public void automatizarCálculos()
        {
			object tipo = 0;
			object[] numeros = new object[1000];
			numeros = palavraDita.Split(' ');
			foreach (object i in numeros)
			{
				tipo = i;
				if (tipo == typeof(int))
				{
					calculadora(int.Parse(tipo.ToString()), int.Parse(tipo.ToString()));
				}
			}

		}//automatizador de cálculos aritiméticos

		public static int calculadora(int number1, int number2)
		{
			resultado = number1 * number2;
			legenda.Text = resultado.ToString();
			return int.Parse(resultado.ToString());
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

				using (WebClient client = new WebClient())
				{
					string res = client.DownloadString(link);
				}
			}
			//em seguida precisamso criar o conector em php para transferir os dados da api cliente acima, para api web
			//<?php

			//	$email = $_REQUEST["email"];
			//	$senha = $_REQUEST["senha"];

			//	$servername = "localhost";
			//	$username = "id20150598_vandadb";
			//	$password = "?riKk^*)^\aBl67^";
			//	$dbname = "id20150598_vandadatabase";

			//	$con2 = new MySqli($servernmae, $username, $password, $dbname);
			//		if($con2->connect_error){
			//			die("error");

			//		}
	
			//	$sql = "INSERT INTO DADOS (email, senha) values ('$email', '$senha')";
	
			//	if($con2->query($sql) === true){
			//		echo("sucess");
			//	}else{
			//		echo("error");
			//	}
			//	$con2->close();
			//	?>
		}
	}
}