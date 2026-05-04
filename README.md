# EvolutionPlay Mobile

Este é um app mobile para o jogo EvolutionPlay, que conecta a um backend ASP.NET Core rodando no seu PC para armazenar os dados.

## Configuração

1. Certifique-se de que o backend ASP.NET Core está rodando no seu PC (porta 5062 por padrão).

2. Instale as workloads necessárias para .NET MAUI:
   ```
   dotnet workload install maui-android maui-ios maui-windows
   ```

3. Instale o Android SDK plataforma 36. Se receber erro de permissão, execute o comando como administrador:
   ```
   dotnet build -t:InstallAndroidDependencies -f net10.0-android "-p:AndroidSdkDirectory=C:\\Program Files (x86)\\Android\\android-sdk" "-p:AcceptAndroidSDKLicenses=true"
   ```

4. Abra o projeto EvolutionPlayMobile no Visual Studio ou VS Code.

5. Para rodar no emulador Android, mantenha a URL do backend como `http://10.0.2.2:5062`.

6. Para rodar em um telefone físico, conecte o telefone ao PC ou use a mesma rede Wi-Fi e troque a URL do backend para `http://<IP-do-PC>:5062`.

7. Execute o app no emulador ou dispositivo desejado.
## Backend

O backend é o projeto ASP.NET Core original, modificado para incluir endpoints API.

Para rodar o backend:
```
cd EvolutionPlay.App
dotnet run
```

O backend estará disponível em http://localhost:5062.

## Funcionalidades

- Dashboard com status do jogador
- Registrar treinos
- Economizar dinheiro
- Registrar compras
- E mais ações do jogo

Os dados são salvos no PC via o backend.