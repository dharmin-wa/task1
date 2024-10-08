import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class EncryptionService {

  secretKey = 'b14ca5898a4e4142aace2ea2143a2410';
  iv = CryptoJS.lib.WordArray.create([0, 0, 0, 0], 16);

  encrypt(data: string): string {
    // Convert the key to a WordArray (must be 32 bytes for AES-256)
    const keyWordArray = CryptoJS.enc.Utf8.parse(this.secretKey);

    // Encrypt the plain text using AES with the given key and IV
    const encrypted = CryptoJS.AES.encrypt(data, keyWordArray, {
      iv: this.iv,
      mode: CryptoJS.mode.CBC,  // CBC mode to match your C# code
      padding: CryptoJS.pad.Pkcs7 // PKCS7 padding
    });

    // Return the encrypted data as a Base64 string
    return encrypted.toString();
  }

  decrypt(encryptedData: string): string {
    // Convert the secret key to a WordArray (must be 32 bytes for AES-256)
    const keyWordArray = CryptoJS.enc.Utf8.parse(this.secretKey);

    // Decrypt the encrypted data using AES with the given key and IV
    const decrypted = CryptoJS.AES.decrypt(encryptedData, keyWordArray, {
      iv: this.iv,
      mode: CryptoJS.mode.CBC,  // CBC mode to match your C# code
      padding: CryptoJS.pad.Pkcs7 // PKCS7 padding
    });

    // Convert the decrypted data to a UTF-8 string and return it
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}
