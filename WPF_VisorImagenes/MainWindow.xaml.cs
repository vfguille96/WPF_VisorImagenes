using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_VisorImagenes
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	///

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			//  Deshabilitamos el botón atrás cuando la pos. de la lista de imgs es 0.
			btnAnterior.IsEnabled = false;

			//CargarImagenes();
			SeleccionarCarpeta();
		}

		//  Posición de la imagen actual.
		private int posImg;

		//  Creamos una lista de imágenes.
		private List<BitmapImage> listaImg;

		// Creamos una variable de contador para las veces que hemos volteado una imagen.
		private static int contadorVolteo = 0;

		// Variable ruta de la carpeta a mostrar las imágenes.
		private string rutaDirectorio = @"";

		private void CargarImagenes()
		{
			//string rutaDirectorio = @"E:\Imagenes\basura";
			listaImg = new List<BitmapImage>();
			string[] listaFicheros = Directory.GetFiles(rutaDirectorio);

			foreach (string item in listaFicheros)
			{
				if (System.IO.Path.GetExtension(item) == ".jpg" || System.IO.Path.GetExtension(item) == ".JPG")
				{
					BitmapImage img = new BitmapImage(new Uri(item));
					listaImg.Add(img);
				}

				if (System.IO.Path.GetExtension(item) == ".png" || System.IO.Path.GetExtension(item) == ".PNG")
				{
					BitmapImage img = new BitmapImage(new Uri(item));
					listaImg.Add(img);
				}
			}

			// Hacemos que se muestre la primera imagen en el cuadro.
			imgContenedor.Source = listaImg[posImg];
		}

		#region Eventos

		private void btnAnterior_Click(object sender, RoutedEventArgs e)
		{
			BtnAnterior();
		}

		private void BtnAnterior()
		{
			btnAnterior.IsEnabled = true;
			posImg--;
			imgContenedor.Source = listaImg[posImg];
			btnSiguiente.IsEnabled = true;

			if (posImg == 0)
				btnAnterior.IsEnabled = false;

			if (posImg == listaImg.Count - 1)
				btnSiguiente.IsEnabled = false;
		}

		private void btnSiguiente_Click(object sender, RoutedEventArgs e)
		{
			BtnSiguiente();
		}

		private void BtnSiguiente()
		{
			posImg++;
			imgContenedor.Source = listaImg[posImg];
			btnSiguiente.IsEnabled = true;
			btnAnterior.IsEnabled = true;

			if (posImg == listaImg.Count - 1)
				btnSiguiente.IsEnabled = false;
		}

		/// <summary>
		/// Función que gira las imágenes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnVoltear_Click(object sender, RoutedEventArgs e)
		{
			if (contadorVolteo == 0 || contadorVolteo % 2 == 0) // Si el contador está a 0 o es par, podemos voltear a la izquierda.
			{
				imgContenedor.RenderTransformOrigin = new Point(0.5, 0.5);
				ScaleTransform flipTrans = new ScaleTransform();
				//flipTrans.ScaleX = -1;
				flipTrans.ScaleY = -1;
				imgContenedor.RenderTransform = flipTrans;

				contadorVolteo++;
			}
			else // Si no, devolvemos la posición original de la imagen volteándola hacia la derecha.
			{
				imgContenedor.RenderTransformOrigin = new Point(0.5, 0.5);
				ScaleTransform flipTrans = new ScaleTransform();
				flipTrans.ScaleX = 1;
				flipTrans.ScaleY = 1;
				imgContenedor.RenderTransform = flipTrans;

				contadorVolteo++;
			}
		}

		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			SeleccionarCarpeta();
		}

		/// <summary>
		/// Función que muestra el diálogo para seleccionar la carpeta donde están las imágenes a mostrar y llama al método para las imágenes
		/// </summary>
		private void SeleccionarCarpeta()
		{
			var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
			if (dialog.ShowDialog(this).GetValueOrDefault())
				rutaDirectorio = dialog.SelectedPath;

			CargarImagenes();
		}

		/// <summary>
		/// Envento que escucha las flechas del teclado para pasar la imagen o retroceder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Right)
				if (posImg != listaImg.Count - 1)
					BtnSiguiente();

			if (e.Key == Key.Left)
				if (posImg != 0)
					BtnAnterior();
		}

		#endregion Eventos
	}
}