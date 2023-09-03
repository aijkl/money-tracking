# money-tracking
不正なお金の流れを検出して通知します  
![image](https://github.com/aijkl/money-tracking/assets/51302983/95691c50-734d-4b34-b377-f5139838fb99)  

## 仕様  
現状は三井住友銀行のみ対応しています。  
1. ATMで引き落とした時に届くメールを解析
2. 定義しておいた期間内の出勤合計額が閾値を超えた場合にDiscordに通知
第三者が居るサーバーに通知する事で圧力がかかり、不要な無駄遣いを防止できます。
仕様上、一回当たりの出金額で判断しないため小刻みに出勤しても検出できます。

## 動かし方
```
$ dotnet run
USAGE:
    MoneyTracker.Cli.dll [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    daemon <HOSTNAME> <MAIL_ADDRESS> <PASSWORD> <SENDER_ADDRESS> <THRESHOLD_YEN> <DAY_COUNT> <INTERVAL_MS>    Starts the daemon
```

## サンプル
```
dotnet run daemon imap.gmail.com メールアドレス パスワード smbc_service@dn.smbc.co.jp 3000 3 1800000 --webhook-url https://discord.com/api/webhooks/1147531363805831328/XXXXXXXXXXXXX
```
