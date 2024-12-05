// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a 3D wall object in a Three.js scene, 
//              which consists of six faces (front, rear, left, right, top) with a 
//              texture applied to the front and rear faces. 
//              Shadows are also enabled for all faces of the wall.
// Adapted from: Basic_Thumb_Raiser (SGRAI) 
// ============================================================================

import * as THREE from "three";

/*
 * parameters = {
 *  textureUrl: String, // URL of the texture to apply to the wall
 * }
 */

export default class Wall {
    /**
     * Constructor for the Wall class.
     * 
     * @param {Object} parameters - Configuration object for the wall.
     */
    constructor(parameters) {
        // Destructure the parameters object to assign individual values
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }

        // Load the texture for the wall
        const texture = new THREE.TextureLoader().load(this.textureUrl);
        texture.colorSpace = THREE.SRGBColorSpace;
        texture.magFilter = THREE.LinearFilter;
        texture.minFilter = THREE.LinearMipmapLinearFilter;

        // Create a group to hold all the faces of the wall
        this.object = new THREE.Group();

        // Define the height and width of the wall
        const height = 5;                       // Height of the wall
        const width = 1;                        // Width of the wall (can be adjusted as needed)
        const halfheight = (height / 2 - 0.5);  // Offset for positioning the faces

        // Create the front face (a rectangle)
        let geometry = new THREE.PlaneGeometry(width, height);
        let material = new THREE.MeshPhongMaterial({ color: 0xD1D8E2 });
        let face = new THREE.Mesh(geometry, material);
        face.position.set(0.0, halfheight, 0.025); // Position the front face slightly in front to avoid z-fighting
        face.castShadow = true;
        face.receiveShadow = true;
        this.object.add(face);

        // Create the rear face (a rectangle), similar to the front face
        face = new THREE.Mesh().copy(face, false);
        face.rotateY(Math.PI);                     // Rotate the face 180 degrees along the Y-axis
        face.position.set(0.0, halfheight, -0.025);// Position the rear face slightly behind to avoid z-fighting
        this.object.add(face);

        // Create the left face (a rectangle), representing the left side of the wall
        geometry = new THREE.PlaneGeometry(0.05, height);
        material = new THREE.MeshPhongMaterial({ color: 0xD1D8E2 });
        face = new THREE.Mesh(geometry, material);
        face.position.set(-width / 2, halfheight, 0); // Position the left face to the left of the center
        face.rotateY(Math.PI / 2);                    // Rotate the face 90 degrees along the Y-axis
        face.castShadow = true;
        face.receiveShadow = true;
        this.object.add(face);

        // Create the right face (a rectangle), representing the right side of the wall
        face = new THREE.Mesh(geometry, material);
        face.position.set(width / 2, halfheight, 0); // Position the right face to the right of the center
        this.object.add(face);

        // Create the top face (a rectangle), representing the top of the wall
        geometry = new THREE.PlaneGeometry(width, 0.05);
        face = new THREE.Mesh(geometry, material);
        face.position.set(0, height - 0.5, 0);  // Position the top face at the top of the wall
        face.rotateX(-Math.PI / 2);             // Rotate the face 90 degrees along the X-axis
        face.castShadow = true;
        face.receiveShadow = true;
        this.object.add(face);
    }
}
