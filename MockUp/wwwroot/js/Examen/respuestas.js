//arreglo que contiene las respuestas correctas
let correctas = [3, 1, 2, 2, 3];

//arreglo donde se gaurdan las respuestas del usuario
let opcion_elegida = [];
let cantidad_correctas = 0;

//funcion que toma el num de pregunta y el input elegido de esa pregunta
function respuesta(num_pregunta, seleccionada) {
    //se guarda la respuesta
    opcion_elegida[num_pregunta] = seleccionada.value;

    id = "p" + num_pregunta;

    labelos = document.getElementById(id).childNodes;
    labels[3].style.backgroundColor = "white";
    labels[5].style.backgroundColor = "white";
    labels[7].style.backgroundColor = "white";

    //se pinta el label seleccionado
    seleccionada.parentNode.style.backgroundColor = "cec0fc";
}

//funcion para saber cuantas estuvieran correctas
function corregir() {
    cantidad_correctas = 0;
    for (i = 0; i < correctas.length; i++) {
        if (correctas[i] == opcion_elegida[i]) {
            cantidad_correctas++;
        }
    }

    docuemnt.getElementById("resultado").innerHTML = cantidad_correctas;
}