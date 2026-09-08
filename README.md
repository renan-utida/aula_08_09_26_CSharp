# Curso de C#

Repositório-modelo do curso de **C#** ministrado pelo **Prof. Cassiolato**.

Cada aula tem uma atividade prática. Este repositório mantém uma pasta
por aula em `sources/`. O objetivo é você acumular um portfólio de
exercícios versionado no seu GitHub ao longo das aulas.

---

## 🚀 Como começar

1. Clique em **"Use this template"** no topo do repositório (não use fork).
2. Dê um nome ao seu novo repositório (sugestão: `fiap-csharp`).
3. Clone localmente:
   ```
   git clone https://github.com/SEU-USUARIO/fiap-csharp
   cd fiap-csharp
   ```
4. Verifique o .NET SDK instalado:
   ```
   dotnet --version
   ```
   Se o comando não for reconhecido, instale o .NET SDK a partir de
   https://dot.net/download (qualquer versão suportada).

---

## 📁 Estrutura do repositório

```
/
├── .editorconfig            → regras de estilo do C# (aplicadas pela IDE)
├── .gitattributes           → fixa quebras de linha em LF
├── .gitignore               → padrão .NET
├── Directory.Build.props    → configurações aplicadas a todos os projetos
├── .github/workflows/       → CI (GitHub Actions)
└── sources/
    └── aula-NN/             → uma pasta por aula
```

---

## ✏️ Como adicionar a atividade de uma nova aula

No terminal, na raiz do repositório:

```
dotnet new console -o sources/aula-01
cd sources/aula-01
dotnet run
```

Isso cria um novo projeto de console dentro de `sources/aula-01/`.
Repita com `aula-02`, `aula-03`, etc.

---

## ✅ Verificação automática de qualidade

Toda vez que você fizer `push` ou abrir um pull request, o
**GitHub Actions** executa:

1. restore e build de todos os projetos em `sources/`;
2. analisadores oficiais do .NET e regras do `.editorconfig`;
3. verificação de formatação;
4. testes automatizados, quando existirem;
5. Gitleaks e verificações de segredos;
6. arquivos que não podem ser enviados, como `bin`, `obj` e `.vs`;
7. geração de uma pontuação de qualidade de 0 a 100.

Enquanto ainda não existir nenhum projeto `.csproj` em `sources/`, o
relatório mostra **não avaliado** em vez de atribuir uma pontuação.

O workflow falha somente em problemas bloqueantes:

- erro de restore ou build;
- segredo versionado;
- arquivo gerado ou proibido rastreado pelo Git.

Warnings de estilo, boas práticas e testes existentes são apresentados
no relatório, mas não bloqueiam o envio.

### Relatórios

O job publica:

- resumo legível na página do GitHub Actions;
- `report.json` com schema versionado para ferramentas;
- `report.md` para leitura direta e uso com LLMs;
- logs e relatórios brutos como artefato.

Detalhes:

- [Schema do JSON](docs/code-quality-report-schema.md)
- [Regras e pontuação](docs/code-quality-rules.md)

### Executar localmente

No PowerShell 7+, a partir da raiz:

```powershell
./scripts/Invoke-CodeQuality.ps1
```

Os arquivos são gerados em `artifacts/code-quality/`.

O Gitleaks é obrigatório no CI. Se ele não estiver instalado localmente,
o script apresenta um aviso e continua com as verificações próprias.

### Testar o pipeline

Os testes não dependem de Pester nem de pacotes adicionais:

```powershell
./tests/Invoke-CodeQuality.Unit.Tests.ps1
./tests/Invoke-CodeQuality.E2E.ps1
```

O E2E cria repositórios temporários e ferramentas simuladas, valida os
relatórios contra o schema JSON e remove todas as fixtures ao terminar.

---

## 🔧 IDE

O curso utiliza o **Visual Studio 2022 ou 2026**.

- O `.editorconfig` deste repositório é aplicado automaticamente pelo Visual Studio ao salvar.

---

## 📚 Guia rápido de estilo (aplicado pelo `.editorconfig`)

- `PascalCase` — classes, métodos, propriedades, enums
- `camelCase` — variáveis locais e parâmetros
- `_camelCase` — campos privados
- `IPascalCase` — interfaces sempre começam com `I`
- 4 espaços de indentação
- Chaves sempre em nova linha em métodos e classes
- `using` fora do `namespace`, ordenados com `System` primeiro
- Valores monetários usam `decimal` (nunca `double`)
- Warnings são orientações: leia o relatório e corrija primeiro os
  bloqueantes, depois os itens de maior impacto.

Sua IDE aplica essas regras automaticamente ao salvar.

---

## 🆘 Problemas comuns

**"`dotnet` não é reconhecido"**
→ Instale o .NET SDK (qualquer versão suportada): https://dot.net/download

**"Erro de encoding" ao clonar**
→ Configure git para usar LF:
```
git config --global core.autocrlf input
```

**"Meu commit apareceu com nome errado"**
→ Configure git com seu email do GitHub:
```
git config --global user.name "Seu Nome"
git config --global user.email "voce@exemplo.com"
```

---

*Bom código.*
