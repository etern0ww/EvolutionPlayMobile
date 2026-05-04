# EvolutionPlay Mobile

Este é o app mobile do jogo EvolutionPlay, desenvolvido em .NET MAUI para Android, iOS, MacCatalyst e Windows.

## Visão geral

O app oferece:
- dashboard de progresso do jogador
- loja com itens e restrições de classe/nível
- crafting de itens e criação de poções
- inventário com itens comprados
- distribuição de pontos de habilidade
- chatbot integrado para ajudar com mecânicas do jogo

## Recursos principais

- Navegação por abas: Home, Loja, Craft, Poções, Inventário e Chatbot
- Atributos: Força, Agilidade, Vitalidade, Inteligência, Velocidade e Resistência
- Regras de compra por nível e por classe
- Pontos de habilidade liberados ao subir de nível
- Persistência de estado local no dispositivo

## Requisitos

- .NET 10 SDK
- Workloads MAUI instaladas:
  ```powershell
  dotnet workload install maui-android maui-ios maui-windows
  ```
- Android SDK com platform-tools e build-tools

## Build e execução

1. Abra `EvolutionPlayMobile\EvolutionPlayMobile` no Visual Studio ou VS Code.
2. Restaure pacotes:
   ```powershell
   dotnet restore
   ```
3. Compile para Android:
   ```powershell
   dotnet build -f net10.0-android
   ```
4. Gere o APK de release:
   ```powershell
   dotnet publish -f net10.0-android -c Release -p:AndroidPackageFormat=apk
   ```

## Instalação no Android

Após publicar, o APK estará em:

`bin\Release\net10.0-android\com.companyname.evolution-Signed.apk`

Instale com ADB:
```powershell
& 'C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe' install -r com.companyname.evolution-Signed.apk
```

## Observações

- O app foi criado para rodar localmente no dispositivo.
- O estado do jogo é salvo localmente, não exigindo backend externo.

## Repositório GitHub

Este projeto está publicado em:

https://github.com/etern0ww/EvolutionPlayMobile
