using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace RSA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RSA rsa;
        /* Encryption */
        private string readDataToEncryption;
        private int readEPublicKeyToEncryption;
        private int readNPublicKeyToEncryption;
        private List<int> encryptionDataList;
        private string decryptionMessage;

        /* Decryption */

        private string readDataToDecryption;
        private int readDPrivateKeyToDecryption;
        private int readNPrivateKeyToDecryption;
        private List<int> decryptionDataList;

        /* Create signature */

        private string readDataCreateSignature;
        private int readDPrivateKeyToCreateSignature;
        private int readNPrivateKeyToCreateSignature;
        private List<int> createSignatureDataList;

        /* Verification document */

        private string readDataSignatureToVerification;
        private string readDataToVerificationSignature;
        private int readEPublicKeyToVerificationSignature;
        private int readNPublicKeyToVerificationSignature;
        private List<int> verificationDataList;
        bool verificationSignatureAnswer = false;


        public MainWindow()
        {
            InitializeComponent();
            rsa = new RSA();
        }

        private void generateKeys_Click(object sender, RoutedEventArgs e)
        {
            try {
                rsa.generateParams();
                paramsRsaKeys.Text = "";
                paramsRsaKeys.Text += "Parametry wygenerowanych kluczy" + Environment.NewLine;
                paramsRsaKeys.Text += "p = " + rsa.p + Environment.NewLine + "q = " + rsa.q + Environment.NewLine;
                paramsRsaKeys.Text += "n = " + rsa.n + Environment.NewLine;
                paramsRsaKeys.Text += "m = " + rsa.m + Environment.NewLine;
                paramsRsaKeys.Text += "e = " + rsa.e + Environment.NewLine;
                paramsRsaKeys.Text += "d = " + rsa.d + Environment.NewLine;
                paramsRsaKeys.Text += Environment.NewLine;
                paramsRsaKeys.Text += "Klucz prywatny (d,n):\t" + "(" + rsa.d + "," + rsa.n + ")" + Environment.NewLine;
                paramsRsaKeys.Text += "Klucz publiczny (e,n):\t" + "(" + rsa.e + "," + rsa.n + ")" + Environment.NewLine;
            }
            catch(Exception ex)
            {
                paramsRsaKeys.Text = "Błąd: " + ex.InnerException;
            }
        }

        private void generateHashWithSignature_Click(object sender, RoutedEventArgs e)
        {
            List<int> hash = rsa.createSignature("siema", 2, 2);

            foreach(var h in hash)
            {
                paramsRsaKeys.Text += "hash " + h;
            }

            if (rsa.verificationSignature(hash, "siema", rsa.e, rsa.n))
                paramsRsaKeys.Text = "Sygnatura poprawna!";
            else
                paramsRsaKeys.Text = "Sygnatura niepoprawna!";

        }

        private void saveKeysToFiles_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    File.WriteAllText(saveFileDialog1.FileName, rsa.getPrivateKey());
                    myStream.Close();
                }
            }
        }

        private void savePrivateKeyToFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (rsa != null)
            {
                if (rsa.d != 0 || rsa.n != 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == true)
                    {
                        File.WriteAllText(saveFileDialog1.FileName, rsa.getPrivateKey());
                    }
                }
                else
                {
                    paramsRsaKeys.Text = "Nie wygenerowano kluczy, aby można było je zapisać!";
                }
            }
            else
            {
                paramsRsaKeys.Text = "Należy najpierw rozpocząć procedurę generowania kluczy!";
            }
        }

        private void savePublicKeyToFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (rsa != null)
            {
                if (rsa.e != 0 || rsa.n != 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == true)
                    {
                        File.WriteAllText(saveFileDialog1.FileName, rsa.getPublicKey());
                    }
                }
                else
                {
                    paramsRsaKeys.Text = "Nie wygenerowano kluczy, aby można było je zapisać!";
                }
            }
            else
            {
                paramsRsaKeys.Text = "Należy najpierw rozpocząć procedurę generowania kluczy!";
            }
        }

        private void encryptionDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (usingGeneratedKeysCheckBox.IsChecked.Value == true)
                    encryptionDataList = rsa.encryption(readDataToEncryption, rsa.e, rsa.n);
                else
                    encryptionDataList = rsa.encryption(readDataToEncryption, readEPublicKeyToEncryption, readNPublicKeyToEncryption);

                encryptionTextBox.Text += "Zaszyfrowana wiadomość: " + Environment.NewLine;

                foreach (var en in encryptionDataList)
                {
                    encryptionTextBox.Text += en + " ";

                }
            }catch(Exception ex)
            {
                encryptionTextBox.Text = "Błąd: " + ex;
            }
        }

        private void decryptionDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (usingGeneratedKeysDecryptionCheckBox.IsChecked.Value == true)
                    decryptionMessage = rsa.decryption(decryptionDataList, rsa.d, rsa.n);
                else
                    decryptionMessage = rsa.decryption(decryptionDataList, readDPrivateKeyToDecryption, readNPrivateKeyToDecryption);

                decryptionTextBox.Text += "Zaszyfrowana wiadomość: " + Environment.NewLine;

                decryptionTextBox.Text += decryptionMessage;
            }catch(Exception ex)
            {
                decryptionTextBox.Text = "Błąd: " + ex;
            }
            
        }

        private void readMessageFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            readDataToEncryption = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readDataToEncryption += File.ReadAllText(openFileDialog1.FileName);
                    encryptionTextBox.Text += "Wczytano dane z: " + openFileDialog1.FileName + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    encryptionTextBox.Text = "Błąd: " + ex.Message;
                }
            }

        }

        private void readPublicKey_Click(object sender, RoutedEventArgs e)
        {
            string readPublicKeyToEncryption = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readPublicKeyToEncryption += File.ReadAllText(openFileDialog1.FileName);
                    string[] lines = Regex.Split(readPublicKeyToEncryption, ";");
                    if(lines.Length == 3)
                    {
                        if (lines[0] != "public")
                            return;
                        int readE;
                        int readN;

                        if (int.TryParse(lines[1], out readE) && int.TryParse(lines[2], out readN))
                        {
                            readEPublicKeyToEncryption = readE;
                            readNPublicKeyToEncryption = readN;
                            encryptionTextBox.Text += "Wczytano klucz publiczny (e,n): (" + readE + "," + readN + ")" + Environment.NewLine; 
                        }
                        else
                        {
                            encryptionTextBox.Text = "Błąd: " + "Nieprawidłowe parametry liczbowe!" + Environment.NewLine;
                            return;
                        }


                    }
                    else
                    {
                        encryptionTextBox.Text = "Błąd: " + "Nieprawidłowe parametry!" + Environment.NewLine;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    encryptionTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void saveEncryptionDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (encryptionDataList != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == true)
                {
                    string encryptionMessage = "";
                    foreach(var data in encryptionDataList)
                    {
                        encryptionMessage += data + " ";
                    }

                    File.WriteAllText(saveFileDialog1.FileName, encryptionMessage);
                }
            }
            else
            {
                paramsRsaKeys.Text = "Należy najpierw rozpocząć procedurę szyfrowania danych!";
            }
        }

        private void readEsncodingMessageFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            readDataToDecryption = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readDataToDecryption += File.ReadAllText(openFileDialog1.FileName);
                    decryptionTextBox.Text += "Wczytano dane z: " + openFileDialog1.FileName + Environment.NewLine;
                    string[] splitData = readDataToDecryption.Split(' ');
                    decryptionDataList = new List<int>();
                    foreach(var data in splitData)
                    {
                        int val;
                        int.TryParse(data, out val);
                        decryptionDataList.Add(val);
                    }
                }
                catch (Exception ex)
                {
                    decryptionTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void readPrivateKey_Click(object sender, RoutedEventArgs e)
        {
            string readPrivateKeyToDecryption = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readPrivateKeyToDecryption += File.ReadAllText(openFileDialog1.FileName);
                    string[] lines = Regex.Split(readPrivateKeyToDecryption, ";");
                    if (lines.Length == 3)
                    {
                        if (lines[0] != "private")
                            return;
                        int readD;
                        int readN;

                        if (int.TryParse(lines[1], out readD) && int.TryParse(lines[2], out readN))
                        {
                            readDPrivateKeyToDecryption = readD;
                            readNPrivateKeyToDecryption = readN;
                            decryptionTextBox.Text += "Wczytano klucz prywatny (d,n): (" + readD + "," + readN + ")" + Environment.NewLine;
                        }
                        else
                        {
                            decryptionTextBox.Text = "Błąd: " + "Nieprawidłowe parametry liczbowe!" + Environment.NewLine;
                            return;
                        }


                    }
                    else
                    {
                        decryptionTextBox.Text = "Błąd: " + "Nieprawidłowe parametry!" + Environment.NewLine;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    decryptionTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void saveDecryptionDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (decryptionDataList != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog1.FileName, decryptionMessage);
                }
            }
            else
            {
                paramsRsaKeys.Text = "Należy najpierw rozpocząć procedurę szyfrowania danych!";
            }
        }

        private void usingGeneratedKeysEncryptionCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void usingGeneratedKeysDecryptionCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void readPrivateKeySignatureButton_Click(object sender, RoutedEventArgs e)
        {
            string readPrivateKeySignature = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readPrivateKeySignature += File.ReadAllText(openFileDialog1.FileName);
                    string[] lines = Regex.Split(readPrivateKeySignature, ";");
                    if (lines.Length == 3)
                    {
                        if (lines[0] != "private")
                            return;
                        int readD;
                        int readN;

                        if (int.TryParse(lines[1], out readD) && int.TryParse(lines[2], out readN))
                        {
                            readDPrivateKeyToCreateSignature = readD;
                            readNPrivateKeyToCreateSignature = readN;
                            signatureTextBox.Text += "Wczytano klucz prywatny (d,n): (" + readD + "," + readN + ")" + Environment.NewLine;
                        }
                        else
                        {
                            signatureTextBox.Text = "Błąd: " + "Nieprawidłowe parametry liczbowe!" + Environment.NewLine;
                            return;
                        }


                    }
                    else
                    {
                        signatureTextBox.Text = "Błąd: " + "Nieprawidłowe parametry!" + Environment.NewLine;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    signatureTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void readMessageButton_Click(object sender, RoutedEventArgs e)
        {
            readDataCreateSignature = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readDataCreateSignature += File.ReadAllText(openFileDialog1.FileName);
                    signatureTextBox.Text += "Wczytano dane z: " + openFileDialog1.FileName + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    signatureTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void generateSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (useGeneratedDataTocreateSignatureCheckbox.IsChecked.Value == true)
                {
                    createSignatureDataList = rsa.createSignature(readDataCreateSignature, rsa.d, rsa.n);
                }
                else
                {
                    createSignatureDataList = rsa.createSignature(readDataCreateSignature, readDPrivateKeyToCreateSignature, readNPrivateKeyToCreateSignature);
                }

                signatureTextBox.Text += "Zaszyfrowana wiadomość: " + Environment.NewLine;

                foreach (var data in createSignatureDataList)
                {
                    signatureTextBox.Text += data + " ";

                }
           }catch(Exception ex)
            {
                signatureTextBox.Text = "Błąd: " + ex;
            }

}

        private void saveSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            if (createSignatureDataList != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == true)
                {
                    string createSignatureMessage = "";
                    foreach (var data in createSignatureDataList)
                    {
                        createSignatureMessage += data + " ";
                        signatureTextBox.Text += data;
                    }

                    File.WriteAllText(saveFileDialog1.FileName, createSignatureMessage);
                }
            }
            else
            {
                signatureTextBox.Text = "Należy najpierw rozpocząć procedurę szyfrowania danych!";
            }
        }

        private void readPublicKeyVerificationSignature_Click(object sender, RoutedEventArgs e)
        {
            string readPublicKeyToEncryption = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readPublicKeyToEncryption += File.ReadAllText(openFileDialog1.FileName);
                    string[] lines = Regex.Split(readPublicKeyToEncryption, ";");
                    if (lines.Length == 3)
                    {
                        if (lines[0] != "public")
                            return;
                        int readE;
                        int readN;

                        if (int.TryParse(lines[1], out readE) && int.TryParse(lines[2], out readN))
                        {
                            readEPublicKeyToVerificationSignature = readE;
                            readNPublicKeyToVerificationSignature = readN;
                            verificateSignatureTextBox.Text += "Wczytano klucz publiczny (e,n): (" + readE + "," + readN + ")" + Environment.NewLine;
                        }
                        else
                        {
                            verificateSignatureTextBox.Text = "Błąd: " + "Nieprawidłowe parametry liczbowe!" + Environment.NewLine;
                            return;
                        }


                    }
                    else
                    {
                        verificateSignatureTextBox.Text = "Błąd: " + "Nieprawidłowe parametry!" + Environment.NewLine;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    verificateSignatureTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void readSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            readDataSignatureToVerification = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readDataSignatureToVerification += File.ReadAllText(openFileDialog1.FileName);
                    verificateSignatureTextBox.Text += "Wczytano dane z: " + openFileDialog1.FileName + Environment.NewLine;
                    string[] splitData = readDataSignatureToVerification.Split(' ');
                    verificationDataList = new List<int>();
                    foreach (var data in splitData)
                    {
                        int val;
                        int.TryParse(data, out val);
                        verificationDataList.Add(val);
                    }
                }
                catch (Exception ex)
                {
                    verificateSignatureTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }

        private void verificationSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (usingGeneratedKeysVerificationCheckBox.IsChecked.Value == true)
                    verificationSignatureAnswer = rsa.verificationSignature(verificationDataList, readDataToVerificationSignature, rsa.e, rsa.n);
                else
                    verificationSignatureAnswer = rsa.verificationSignature(verificationDataList, readDataToVerificationSignature, readEPublicKeyToVerificationSignature, readNPublicKeyToVerificationSignature);

                if(verificationSignatureAnswer == true)
                    verificateSignatureTextBox.Text += "Dokument poprawny! " + Environment.NewLine;
                else
                    verificateSignatureTextBox.Text += "Dokument sfałszowany! " + Environment.NewLine;


            }
            catch (Exception ex)
            {
                verificateSignatureTextBox.Text = "Błąd: " + ex;
            }

        }

        private void readMessageSignatureVerification_Click(object sender, RoutedEventArgs e)
        {
            readDataToVerificationSignature = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    readDataToVerificationSignature = File.ReadAllText(openFileDialog1.FileName);
                    verificateSignatureTextBox.Text += "Wczytano dane z: " + openFileDialog1.FileName + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    verificateSignatureTextBox.Text = "Błąd: " + ex.Message;
                }
            }
        }
    }
}
