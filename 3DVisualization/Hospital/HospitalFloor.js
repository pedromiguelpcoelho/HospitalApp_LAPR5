// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: ...
// Adapted from: Basic_Thumb_Raiser (SGRAI) 
// ============================================================================

import * as THREE from "three";
import Ground from "./Ground.js";
import Wall from "./Wall.js";
import Door from "./Door.js";
import Bed from "./Bed.js";
import Patient from "./Patient.js";
import Surgeon from "./Surgeon.js";
import Troley from "./Troley.js";
import Tweezer from "./Tweezer.js";
import Syringe from "./Syringe.js";
import WallWithDoorFrame from "./SurgeryDoor.js";
import { OBJLoader } from 'three/addons/loaders/OBJLoader.js';
import { FBXLoader } from 'three/addons/loaders/FBXLoader.js';


/*
* parameters = {
*  url: String,
*  credits: String,
*  scale: Vector3
* }
*/

export default class HospitalFloor {
    constructor(parameters) {
        this.onLoad = function (description) {
            this.objLoader = new OBJLoader();
            this.fbxLoader = new FBXLoader();
            // Store the maze's map and size
            this.map = description.map;
            this.size = description.size;

            // Create a group of objects
            this.object = new THREE.Group();

            // Create the ground
            this.ground = new Ground({ textureUrl: description.groundTextureUrl, size: description.size });
            this.object.add(this.ground.object);

            // Create a wall
            this.wall = new Wall({textureUrl: description.wallTextureUrl, height: 2.0});

            //Create a door
            this.door = new Door({textureUrl: description.doorTextureUrl, height: 2.0});

            // Create a bed
            this.bed = new Bed({textureUrl: description.bedTextureUrl, scale: 0.1});
            
            // Create a patient
            this.patient = new Patient({patientTextureUrl: description.patientTextureUrl, patientModelOBJUrl: description.patientModelOBJUrl});

            // Create a surgeon
            this.surgeon = new Surgeon({surgeonTextureUrl: description.surgeonTextureUrl, surgeonModelOBJUrl: description.surgeonModelOBJUrl});

            // Create a troley
            this.troley = new Troley({troleyTextureUrl: description.troleyTextureUrl, troleyModelOBJUrl: description.troleyModelOBJUrl});

            //Create a tweezer
            this.tweezer = new Tweezer({textureUrl: description.tweezerTextureUrl, scale: 0.1});
            this.syringe = new Syringe({modelUrl: description.syringeTextureUrl, scale: 0.1});


            this.wallWithDoorFrame = new WallWithDoorFrame({ textureUrl: description.surgeryRoomDoorTextureUrl, width: 1, height: 2.0 });
            
            // Build the hospital floor
            let wallObject;
            let doorObject;
            let wallWithDoorFrameObject;
            let door1 = true;

            for (let i = 0; i <= description.size.width; i++) { // In order to represent the eastmost walls, the map width is one column greater than the actual maze width
                for (let j = 0; j <= description.size.height; j++) { // In order to represent the southmost walls, the map height is one row greater than the actual maze height
                    /*
                    * description.map[][] | North wall | West wall
                    * --------------------+------------+-----------
                    *          0          |     No     |     No
                    *          1          |     No     |    Yes
                    *          2          |    Yes     |     No
                    *          3          |    Yes     |    Yes
                    */
                    if (description.map[j][i] == 2 || description.map[j][i] == 3) {
                        wallObject = this.wall.object.clone();
                        wallObject.position.set(i - description.size.width / 2.0 + 0.5, 0.5, j - description.size.height / 2.0);
                        this.object.add(wallObject);
                    }
                    if (description.map[j][i] == 1 || description.map[j][i] == 3) {
                        wallObject = this.wall.object.clone();
                        wallObject.rotateY(Math.PI / 2.0);
                        wallObject.position.set(i - description.size.width / 2.0, 0.5, j - description.size.height / 2.0 + 0.5);
                        this.object.add(wallObject);
                    }
                    if (description.map[j][i] == 5) {

                        this.fixSmallGapNorth(i, j, description);
                        
                        doorObject = this.door.object.clone();
                        doorObject.position.set(i - description.size.width / 2.0 + 0.5, 0.5, j - description.size.height / 2.0);
                        this.object.add(doorObject);
                    }

                    if(description.map[j][i] == 6) {

                        this.fixSmallGapWest(i, j, description);
                        
                        wallWithDoorFrameObject = this.wallWithDoorFrame.object.clone();
                    if (door1) {
                        // First Door
                        wallWithDoorFrameObject.rotateY(Math.PI / 2.0);
                        wallWithDoorFrameObject.position.set(i - description.size.width / 2.0, 0.5, j - description.size.height / 2.0 + 0.5);
                    } else {
                        // Segunda porta espelhada
                        wallWithDoorFrameObject.scale.x = -1; // Espelha a segunda porta no eixo X
                        wallWithDoorFrameObject.rotateY(Math.PI / 2.0);
                        wallWithDoorFrameObject.position.set(i - description.size.width / 2.0, 0.5, j - description.size.height / 2.0 + 0.5);
                    }

                        this.object.add(wallWithDoorFrameObject);
                        door1 = !door1; // Alterna o valor de door1
                    }
                    if(description.map[j][i] == 7) {

                        this.fixSmallGapWest(i, j, description);
                        wallWithDoorFrameObject = this.wallWithDoorFrame.object.clone();

                        if (door1) {
                            // Primeira porta
                            wallWithDoorFrameObject.position.set(i - description.size.width / 2.0 - 0.5, 0.5, j - description.size.height / 2.0 );
                        } else {
                            // Segunda porta espelhada
                            wallWithDoorFrameObject.scale.x = -1; // Espelha a segunda porta no eixo X
                            wallWithDoorFrameObject.rotateY(Math.PI);
                            wallWithDoorFrameObject.position.set(i - description.size.width / 2.0 - 0.5, 0.5, j - description.size.height / 2.0 + 1);
                        }

                        this.object.add(wallWithDoorFrameObject);
                        door1 = !door1; // Alterna o valor de porta1
                    }
                    if(description.map[j][i] == 8) {

                        this.fixSmallGapWest(i, j, description);

                        wallWithDoorFrameObject = this.wallWithDoorFrame.object.clone();

                        if (door1) {
                            // Primeira porta
                            wallWithDoorFrameObject.rotateY(Math.PI);
                            wallWithDoorFrameObject.position.set(i - description.size.width / 2.0 + 0.5, 0.5, j - description.size.height / 2.0 );
                        } else {
                            // Segunda porta espelhada
                            wallWithDoorFrameObject.scale.x = -1; // Espelha a segunda porta no eixo X
                            wallWithDoorFrameObject.position.set(i - description.size.width / 2.0 + 0.5, 0.5, j - description.size.height / 2.0 + 1);
                        }

                        this.object.add(wallWithDoorFrameObject);
                        door1 = !door1; // Alterna o valor de porta1
                    }
                     
                    if (description.map[j][i] == 9) {
                        this.clonarBed(i, j, description); // Chama o novo método para clonar a Bed
                        this.clonarPatient(i, j, description); // Chama o novo método para clonar o paciente
                        this.clonarSurgeon(i, j, description); // Chama o novo método para clonar o surgeon
                        this.cloneTroley(i, j, description); // Chama o novo método para clonar o troley
                        this.clonarTweezer(i, j, description); // Chama o novo método para clonar o tweezer
                        this.clonarSyringe(i, j, description); // Chama o novo método para clonar o syringe
                       // this.clonarAdereco(i, j, description); // Chama o novo método para clonar o adereço
                    }

                    if (description.map[j][i] == 4) {
                       this.clonarBed(i, j, description); 
                       this.cloneTroley(i, j, description);
                    }
                }
            }
            

            this.object.scale.set(this.scale.x, this.scale.y, this.scale.z);
            this.loaded = true;
        }

        this.onProgress = function (url, xhr) {
            console.log("Resource '" + url + "' " + (100.0 * xhr.loaded / xhr.total).toFixed(0) + "% loaded.");
        }

        this.onError = function (url, error) {
            console.error("Error loading resource " + url + " (" + error + ").");
        }

        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        this.loaded = false;

        // The cache must be enabled; additional information available at https://threejs.org/docs/api/en/loaders/FileLoader.html
        THREE.Cache.enabled = true;

        // Create a resource file loader
        const loader = new THREE.FileLoader();

        // Set the response type: the resource file will be parsed with JSON.parse()
        loader.setResponseType("json");

        // Load a maze description resource file
        loader.load(
            //Resource URL
            this.url,
            // onLoad callback
            description => this.onLoad(description),
            // onProgress callback
            xhr => this.onProgress(this.url, xhr),
            // onError callback
            error => this.onError(this.url, error)
        );
    }

    // Convert cell [row, column] coordinates to cartesian (x, y, z) coordinates
    cellToCartesian(position) {
        return new THREE.Vector3((position[1] - this.size.width / 2.0 + 0.5) * this.scale.x, 0.0, (position[0] - this.size.height / 2.0 + 0.5) * this.scale.z)
    }

    fixSmallGapWest(i,j, description) {

        this.wall = new Wall({ textureUrl: description.wallTextureUrl, height: 2.0 });
        let wallObject;

        wallObject = this.wall.object.clone();
        wallObject.rotateY(Math.PI / 2.0);

        const wallHeightScale = 0.2; // Define o fator de escala para a altura da parede
        wallObject.scale.set(1, wallHeightScale, 1);
        
        wallObject.position.set(i - description.size.width / 2.0, 4.1, j - description.size.height / 2.0 + 0.5);

        this.object.add(wallObject);
    }

    fixSmallGapNorth(i,j, description) {

        this.wall = new Wall({ textureUrl: description.wallTextureUrl, height: 2.0 });
        let wallObject;

        wallObject = this.wall.object.clone();

        const wallHeightScale = 0.2; // Define o fator de escala para a altura da parede
        wallObject.scale.set(1, wallHeightScale, 1);
        
        wallObject.position.set(i - description.size.width / 2.0 + 0.5, 4.1, j - description.size.height / 2.0);

        this.object.add(wallObject);
    }

    clonarBed(i, j, description) {
        // Wait until the object has been loaded, then add it to the scene
        this.bed.objLoader.load(this.bed.textureUrl, (object) => {
            const bedObject = this.bed.object.clone(); // Clone of the object
            // Adjust the bed position to be in the center of the room
            bedObject.position.set(
                i - description.size.width / 2.0,   //Position along the x-axis, centered by half the bed's width
                1,                                  // Position along the y-axis, elevating the bed slightly above the ground
                j - description.size.height / 2     //Position along the z-axis, centered by half the bed's height
            );
            this.object.add(bedObject); // Add the bed object to the scene
        });
    }

    clonarSyringe(i, j, description) {
        // Wait until the object has been loaded, then add it to the scene
        this.syringe.fbxLoader.load(this.syringe.modelUrl, (object) => {
            const syringeObject = this.syringe.object.clone(); // Clone of the object
            // Adjust the syringe position to be in the center of the room
            syringeObject.position.set(
                i - description.size.width / 1.77,   //Position along the x-axis, centered by half the syringe's width
                1.65,                                  // Position along the y-axis, elevating the syringe slightly above the ground
                j - description.size.height / 2.22     //Position along the z-axis, centered by half the syringe's height
            );
            this.object.add(syringeObject); // Add the syringe object to the scene
        });
    }

    clonarTweezer(i, j, description) {
        // Wait until the object has been loaded, then add it to the scene
        this.tweezer.objLoader.load(this.tweezer.textureUrl, (object) => {
            const tweezerObject = this.tweezer.object.clone(); // Clone of the object
            // Adjust the tweezer position to be in the center of the room
            tweezerObject.position.set(
                i - description.size.width / 1.8,   //Position along the x-axis, centered by half the tweezer's width
                1.65,                                  // Position along the y-axis, elevating the tweezer slightly above the ground
                j - description.size.height / 2.35     //Position along the z-axis, centered by half the tweezer's height
            );
            this.object.add(tweezerObject); // Add the tweezer object to the scene
        });
    }

    clonarSurgeon(i, j, description) {
        // Aguarde até que o objeto tenha sido carregado, depois adicione à cena
        this.surgeon.objLoader.load(this.surgeon.surgeonModelOBJUrl, (object) => {
            const surgeonObject = this.surgeon.object.clone(); // Clone do objeto
            // Ajustar a posição da surgeon para ficar perto da parede
            surgeonObject.position.set(
                i - description.size.width / 1.75, // Ajuste a posição conforme necessário
                0,
                j - description.size.height / 2
            );
            surgeonObject.rotateY(Math.PI/2);

            this.object.add(surgeonObject); // Adiciona o objeto surgeon à cena
        });
    }

    cloneTroley(i, j, description) {
        // Aguarde até que o objeto tenha sido carregado, depois adicione à cena
        this.troley.objLoader.load(this.troley.troleyModelOBJUrl, (object) => {
            const troleyObject = this.troley.object.clone(); // Clone do objeto
            // Ajustar a posição da troley para ficar perto da parede
            troleyObject.position.set(
                i - description.size.width / 1.78, // Ajuste a posição conforme necessário
                0.5,
                j - description.size.height / 2.3
            );
            troleyObject.rotateY(Math.PI/2);

            this.object.add(troleyObject); // Adiciona o objeto troley à cena
        });
    }

    clonarPatient(i, j, description) {
        // Aguarde até que o objeto tenha sido carregado, depois adicione à cena
        this.patient.objLoader.load(this.patient.patientModelOBJUrl, (object) => {
            const patientObject = this.patient.object.clone(); // Clone do objeto
            // Ajustar a posição do paciente para ficar perto da parede
            patientObject.position.set(
                i - description.size.width / 2.0, // Ajuste a posição conforme necessário
                1.55,
                j - description.size.height / 2.05
            );
            patientObject.rotateY(Math.PI);
            this.object.add(patientObject); // Adiciona o objeto paciente à cena
        });


    }
    // Convert cartesian (x, y, z) coordinates to cell [row, column] coordinates
    cartesianToCell(position) {
        return [Math.floor(position.z / this.scale.z + this.size.height / 2.0), Math.floor(position.x / this.scale.x + this.size.width / 2.0)];
    }

    distanceToWestWall(position) {
        const indices = this.cartesianToCell(position);
        if (this.map[indices[0]][indices[1]] == 1 || this.map[indices[0]][indices[1]] == 3) {
            return position.x - this.cellToCartesian(indices).x + this.scale.x / 2.0;
        }
        return Infinity;
    }

    distanceToEastWall(position) {
        const indices = this.cartesianToCell(position);
        indices[1]++;
        if (this.map[indices[0]][indices[1]] == 1 || this.map[indices[0]][indices[1]] == 3) {
            return this.cellToCartesian(indices).x - this.scale.x / 2.0 - position.x;
        }
        return Infinity;
    }

    distanceToNorthWall(position) {
        const indices = this.cartesianToCell(position);
        if (this.map[indices[0]][indices[1]] == 2 || this.map[indices[0]][indices[1]] == 3) {
            return position.z - this.cellToCartesian(indices).z + this.scale.z / 2.0;
        }
        return Infinity;
    }

    distanceToSouthWall(position) {
        const indices = this.cartesianToCell(position);
        indices[0]++;
        if (this.map[indices[0]][indices[1]] == 2 || this.map[indices[0]][indices[1]] == 3) {
            return this.cellToCartesian(indices).z - this.scale.z / 2.0 - position.z;
        }
        return Infinity;
    }
}