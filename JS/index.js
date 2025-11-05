import crypto from "crypto";

function decrypt(cipherText, key) {
  const [ivBase64, dataBase64] = cipherText.split(":");
  if (!ivBase64 || !dataBase64) {
    throw new Error("Texto criptografado inválido");
  }

  const iv = Buffer.from(ivBase64, "base64");
  const encryptedText = Buffer.from(dataBase64, "base64");

  const decipher = crypto.createDecipheriv(
    "aes-256-cbc",
    Buffer.from(key, "utf8"),
    iv
  );

  let decrypted = decipher.update(encryptedText, undefined, "utf8");
  decrypted += decipher.final("utf8");

  return decrypted;
}

const key = "12345678901234567890123456789012";
const encrypted = "NQnhSfYRCRBQgvhuripT7w==:LdKbdgCrXsspqVw/GVZzLQ==";
const decrypted = decrypt(encrypted, key);

console.log("Senha:", decrypted);
