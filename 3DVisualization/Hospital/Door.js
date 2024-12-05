// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: ...
// ============================================================================

import * as THREE from "three";

/*
 * parameters = {
 *  textureUrl: String, // URL of the texture to be applied to the door
 *  width: Number, // Width of the door
 *  height: Number // Height of the door
 * }
 */

export default class Door {
    /**
     * Constructor for the Door class.
     * 
     * @param {Object} parameters - Configuration object for the Door.
     */
    constructor(parameters) {
        // Assign the provided parameters to the instance dynamically
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }

        // Load the texture for the door
        const texture = new THREE.TextureLoader().load(this.textureUrl);
        texture.colorSpace = THREE.SRGBColorSpace; // Set the texture color space
        texture.magFilter = THREE.LinearFilter; // Set the texture's magnification filter
        texture.minFilter = THREE.LinearMipmapLinearFilter; // Set the texture's minification filter

        // Create a group to hold the door and its components (front and back faces)
        this.object = new THREE.Group();

        // Define the total wall height and calculate the remaining height for the door
        const wallHeight = 5; // Height of the wall
        const doorHeight = 4; // Height of the door
        const missingHeight = wallHeight - doorHeight;

        // Create the front face of the door
        let geometry = new THREE.PlaneGeometry(this.width, doorHeight);
        let material = new THREE.MeshPhongMaterial({ color: 0xffffff, map: texture, transparent: true });
        let face = new THREE.Mesh(geometry, material);

        // Position the front face of the door so that its base is on the ground
        face.position.set(0, 1.5, 0);
        face.castShadow = true; // Enable shadow casting for the front face
        face.receiveShadow = true; // Enable shadow receiving for the front face
        this.object.add(face);

        // Create the back face of the door
        const backFace = new THREE.Mesh(geometry, material);
        backFace.position.set(0, 1.5, 0);
        backFace.rotation.y = Math.PI; // Rotate the back face 180 degrees
        backFace.castShadow = true; // Enable shadow casting for the back face
        backFace.receiveShadow = true; // Enable shadow receiving for the back face
        this.object.add(backFace);
    }
}
