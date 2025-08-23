import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

// Backend target passed by Aspire or fallback to local
const target = env["services__turneromedico-server__https__0"]
  ?? env.VITE_API_BASE_URL
  ?? 'https://localhost:7120';

// Optional HTTPS for Vite dev server (disabled by default to work with Aspire HTTP endpoint)
let httpsConfig: false | { key: Buffer; cert: Buffer } = false;
if (env.VITE_USE_HTTPS === 'true') {
  const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
      ? `${env.APPDATA}/ASP.NET/https`
      : `${env.HOME}/.aspnet/https`;

  const certificateName = 'turneromedico.client';
  const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
  const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

  if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
  }

  if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    const result = child_process.spawnSync('dotnet', [
      'dev-certs',
      'https',
      '--export-path',
      certFilePath,
      '--format',
      'Pem',
      '--no-password',
    ], { stdio: 'inherit' });
    if (result.status !== 0) {
      throw new Error('Could not create certificate.');
    }
  }

  httpsConfig = {
    key: fs.readFileSync(keyFilePath),
    cert: fs.readFileSync(certFilePath),
  };
}

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  server: {
    proxy: {
      '^/api': {
        target,
        changeOrigin: true,
        secure: false,
      },
    },
    host: true,           // expone en la red (0.0.0.0)
    strictPort: true,     // no cambiar de puerto automáticamente
    port: Number.parseInt(env.PORT || env.DEV_SERVER_PORT || '5173', 10),
    https: httpsConfig,
  },
});
