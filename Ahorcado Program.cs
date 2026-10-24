using System;
using System.Collections.Generic;
using System.Security.Cryptography;

// Lista global de palabras.
    List<string> palabras = new List<string>();

    // Variable global con el número de intentos restantes.
    int intentosRestantes = 6;

        Hangman();

    // Función principal del juego.
    void Hangman()
    {
        intentosRestantes = 6;
        MostrarCabecera();
        PrecargarPalabras();

        string palabraAdivinar = ElegirModoDeJuego();
        string palabraOculta = OcultarPalabra(palabraAdivinar);

        // Guarda las letras ya introducidas.
        string letrasUsadas = "";

        while (IntentosRestantes() > 0 && !HasGanado(palabraAdivinar, palabraOculta))
        {
            Console.WriteLine();
            Console.WriteLine("Palabra:");
            DibujarLineas(palabraOculta);

            Console.WriteLine();
            Console.WriteLine("Intentos restantes: " + IntentosRestantes());

            if (letrasUsadas.Length > 0)
            {
                Console.WriteLine("Letras usadas: " + letrasUsadas);
            }

            char letra = SolicitarLetra();

            // Evita penalizar si la letra ya se había introducido.
            if (letrasUsadas.Contains(letra.ToString()))
            {
                Console.Clear();
                Console.WriteLine("Ya habías utilizado esa letra. Prueba con otra.");
                continue;
            }

            letrasUsadas += letra + " ";

            if (ComprobarLetra(palabraAdivinar, letra))
            {
                Console.Clear();
                Console.WriteLine("¡Correcto! La letra está en la palabra.");
                palabraOculta = ReemplazarLineas(
                    palabraAdivinar,
                    palabraOculta,
                    letra
                );
            }
            else
            {
                Console.WriteLine("La letra no está en la palabra.");
                DecrementarVidas();
            }
        }

        Console.WriteLine();
        Console.WriteLine("----------------------------------------");

        if (HasGanado(palabraAdivinar, palabraOculta))
        {
            DibujarLineas(palabraOculta);
            Console.WriteLine();
            Console.WriteLine("¡ENHORABUENA! Has ganado.");
            Console.WriteLine("La palabra era: " + palabraAdivinar);
        }
        else
        {
            Console.WriteLine("Te has quedado sin intentos.");
            Console.WriteLine("Has perdido.");
            Console.WriteLine("La palabra era: " + palabraAdivinar);
            Console.Beep(400, 2000);
    }

        Console.WriteLine("----------------------------------------");
        Console.WriteLine();
        Console.WriteLine("Pulsa una tecla para seguir jugando...");
        Console.ReadKey();
        Hangman();
    }


    // Muestra la cabecera del juego.
    static void MostrarCabecera()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("          JUEGO DEL AHORCADO");
        Console.WriteLine("========================================");
        Console.WriteLine("Tienes 6 oportunidades para acertar.");
        Console.WriteLine("Debes introducir una sola letra.");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    // Precarga 10 palabras en la lista.
    void PrecargarPalabras()
    {
        palabras.Clear();

        palabras.Add("ordenador");
        palabras.Add("programacion");
        palabras.Add("teclado");
        palabras.Add("pantalla");
        palabras.Add("internet");
        palabras.Add("servidor");
        palabras.Add("memoria");
        palabras.Add("codigo");
        palabras.Add("terminal");
        palabras.Add("software");
    }

    // Permite elegir entre palabras precargadas o palabras introducidas manualmente.
    string ElegirModoDeJuego()
    {
        Console.WriteLine("Selecciona el modo de juego:");
        Console.WriteLine("1 - Usar una palabra aleatoria de las 10 precargadas.");
        Console.WriteLine("2 - Introducir una o más palabras manualmente.");
        Console.WriteLine("0 - Salir.");

    Console.Write("Opción: ");

        string opcion = Console.ReadLine();

        while (opcion != "1" && opcion != "2" && opcion != "0")
        {
            Console.Write("Opción incorrecta. Introduce 1, 2 o 0 para Salir: ");
            opcion = Console.ReadLine();
        }
        if (opcion == "0")
        {
        Environment.Exit(0);
        }
        if (opcion == "1")
        {
            return SeleccionarPalabraAleatoria();
        }

        IntroducirPalabrasManualmente();

        // Limpia la pantalla para que quien vaya a jugar no vea las palabras.
        Console.Clear();
        MostrarCabecera();

        return SeleccionarPalabraAleatoria();
    }

    // Extra: permite introducir una o varias palabras manualmente.
    void IntroducirPalabrasManualmente()
    {
        palabras.Clear();

        Console.WriteLine();
        Console.WriteLine("Introduce una o varias palabras.");
        Console.WriteLine("Cuando termines, escribe FIN.");
        Console.WriteLine("Las palabras deben contener solamente letras.");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Palabra: ");
            string entrada = Console.ReadLine();

            if (entrada == null)
            {
                continue;
            }

            entrada = entrada.Trim().ToLower();

            if (entrada == "fin")
            {
                if (palabras.Count == 0)
                {
                    Console.WriteLine("Debes introducir al menos una palabra.");
                    continue;
                }

                break;
            }

            if (!EsPalabraValida(entrada))
            {
                Console.WriteLine("Palabra no válida. Escribe únicamente letras.");
                continue;
            }

            palabras.Add(entrada);
            Console.WriteLine("Palabra guardada.");
        }
    }

    // Comprueba que una palabra solamente contenga letras.
    static bool EsPalabraValida(string palabra)
    {
        if (palabra.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < palabra.Length; i++)
        {
            if (!char.IsLetter(palabra[i]))
            {
                return false;
            }
        }

        return true;
    }

    // Devuelve una palabra aleatoria de la lista.
    string SeleccionarPalabraAleatoria()
    {
        Random aleatorio = new Random();
        int posicion = aleatorio.Next(0, palabras.Count);

        return palabras[posicion];
    }

    // Sustituye todos los caracteres de una palabra por "_".
    static string OcultarPalabra(string palabra)
    {
        string palabraOculta = "";

        for (int i = 0; i < palabra.Length; i++)
        {
            palabraOculta += "_";
        }

        return palabraOculta;
    }

    // Dibuja la palabra dejando un espacio entre caracteres.
    static void DibujarLineas(string texto)
    {
        for (int i = 0; i < texto.Length; i++)
        {
            Console.Write(texto[i] + " ");
        }

        Console.WriteLine();
    }

    // Devuelve los intentos restantes.
    int IntentosRestantes()
    {
        return intentosRestantes;
    }

    // Solicita una única letra al usuario.
    char SolicitarLetra()
    {
        while (true)
        {
            Console.Write("Introduce una letra: ");
            string entrada = Console.ReadLine();

            if (entrada == null)
            {
                Console.WriteLine("Debes introducir una letra.");
                continue;
            }

            entrada = entrada.Trim().ToLower();

            if (entrada.Length != 1)
            {
                Console.WriteLine("Error: solamente puedes introducir una letra.");
                continue;
            }

            if (!char.IsLetter(entrada[0]))
            {
                Console.WriteLine("Error: debes introducir una letra válida.");
                continue;
            }

            return entrada[0];
        }
    }

    // Comprueba si la letra se encuentra dentro de la palabra.
    static bool ComprobarLetra(string palabra, char letra)
    {
        palabra = palabra.ToLower();
        letra = char.ToLower(letra);

        for (int i = 0; i < palabra.Length; i++)
        {
            if (palabra[i] == letra)
            {
                return true;
            }
        }

        return false;
    }

    // Decrementa el número de vidas.
    void DecrementarVidas()
    {
    intentosRestantes--;
    Console.Beep();
    Console.Clear();

    Console.WriteLine("  +---+");
    Console.WriteLine("  |   |");
    switch (intentosRestantes)
        {
            case 5:
            Console.WriteLine("  O   |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            break;
            case 4:
            Console.WriteLine("  O   |");
            Console.WriteLine("  |   |");
            Console.WriteLine("      |");
            break;
            case 3:
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|   |");
            Console.WriteLine("      |");
            break;
            case 2:
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|\\  |");
            Console.WriteLine("      |");
            break;
            case 1:
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|\\  |");
            Console.WriteLine(" /    |");
            break;
            case 0:
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|\\  |");
            Console.WriteLine(" / \\  |");
            break;
    }
    Console.WriteLine("      |");
    Console.WriteLine("=========");
}


// Reemplaza los "_" por la letra correcta en sus posiciones.
string ReemplazarLineas(
        string palabraAdivinar,
        string palabraActual,
        char letra
    )
    {
        string nuevaPalabra = "";

        palabraAdivinar = palabraAdivinar.ToLower();
        letra = char.ToLower(letra);

        for (int i = 0; i < palabraAdivinar.Length; i++)
        {
            if (palabraAdivinar[i] == letra)
            {
                nuevaPalabra += palabraAdivinar[i];
            }
            else
            {
                nuevaPalabra += palabraActual[i];
            }
        }

        return nuevaPalabra;
    }

    // Comprueba si se ha completado la palabra.
    bool HasGanado(string palabraAdivinar, string palabraActual)
    {
        return palabraAdivinar.ToLower() == palabraActual.ToLower();
    }

