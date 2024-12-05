// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents the ground in a 3D scene, including its 
//              texture and size, using the three.js library.
// Adapted from: Basic_Thumb_Raiser (SGRAI) 
// ============================================================================

import * as THREE from "three";

/*
 * parameters = {
 *  textureUrl: String, // URL of the texture to be applied to the ground
 *  size: Vector2 // Size of the ground (width and height)
 * }
 */

export default class Ground {
    /**
     * Constructor for the Ground class.
     * 
     * @param {Object} parameters - Configuration object for the Ground.
     */
    constructor(parameters) {
        // Assign the provided parameters to the instance dynamically
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }

        // Load the texture for the ground
        const texture = new THREE.TextureLoader().load(this.textureUrl);
        texture.colorSpace = THREE.SRGBColorSpace; // Set the texture color space to sRGB
        texture.wrapS = THREE.RepeatWrapping; // Repeat the texture horizontally
        texture.wrapT = THREE.RepeatWrapping; // Repeat the texture vertically
        texture.repeat.set(this.size.width, this.size.height); // Set the number of times the texture repeats
        texture.magFilter = THREE.LinearFilter; // Set the texture's magnification filter
        texture.minFilter = THREE.LinearMipmapLinearFilter; // Set the texture's minification filter

        // Create a plane geometry for the ground
        const geometry = new THREE.PlaneGeometry(this.size.width, this.size.height);
        const material = new THREE.MeshPhongMaterial({ color: 0xffffff, map: texture }); // Material for the ground with the texture
        this.object = new THREE.Mesh(geometry, material); // Create the mesh for the ground

        // Rotate the ground plane to lie flat on the XZ plane
        this.object.rotateX(-Math.PI / 2.0); // Rotate the plane 90 degrees along the X-axis
        this.object.castShadow = false; // Disable shadow casting for the ground
        this.object.receiveShadow = true; // Enable shadow receiving for the ground
    }
}
