// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a 3D model of a troley, including texture 
//              and OBJ model loading, applied to a Three.js scene.
// ============================================================================

import * as THREE from "three";
import { OBJLoader } from 'three/addons/loaders/OBJLoader.js';

/*
 * parameters = {
 *  troleyTextureUrl: String, // URL of the troley's texture
 *  troleyModelOBJUrl: String, // URL of the troley's OBJ model file
 *  scale: Number, // Optional scale value
 * }
 */

export default class Troley {
    /**
     * Constructor for the troley class.
     * 
     * @param {Object} parameters - Configuration object for the troley.
     */
    constructor(parameters) {
        // Dynamically assign the provided parameters to the instance
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        
        // Create a group to hold the troley model and its components
        this.object = new THREE.Group();

        // Instantiate the OBJLoader to load the troley model
        this.objLoader = new OBJLoader();

        // Instantiate the TextureLoader to load the texture
        this.textureLoader = new THREE.TextureLoader();

        // Load the texture for the troley
        this.texture = this.textureLoader.load(this.troleyTextureUrl);

        // Load the troley OBJ model
        this.objLoader.load(
            this.troleyModelOBJUrl, // URL of the OBJ file
            (object) => {
                // Scale the troley model to the desired size
                object.scale.set(3, 6, 2);

                // Apply the loaded texture to each material of the model
                object.traverse((child) => {
                    if (child.isMesh) {
                        child.material.map = this.texture; // Assign the texture to the material
                        child.material.needsUpdate = true; // Update the material with the new texture
                    }
                });

                // Add the loaded troley object to the group
                this.object.add(object);
            },
            undefined, // onProgress function (not used in this case)
            (error) => {
                // Handle errors during the model loading process
                console.error('An error occurred while loading the troley model:', error);
            }
        );
    }
}
