# Unityでプリンターを扱うライブラリ

## 動作環境・テスト環境

- Windowsのみ対応
- Unity 2017 .2.0f3

## Usage

#### Install
    1. UnityPackageをインポート


#### Test

*RenderTexureを保存して印刷するテストシーン*
1. printToRenderTexシーンを開く
2. SキーでRenderTexureをpngとして保存
3. Pキーで保存したpngを印刷
4. Cキーでジョブのチェック
5. Dキーでジョブのデリート


#### Method

*印刷*

    PrintPhotoUtil.PrintPhoto(filePath, copies, paperWidth, paperHeight, OnEndPrint);

*ジョブのチェック*

    PrintPhotoUtil.JobCheck (OutputHandler, ErrorOutputHanlder, Process_Exit);

*ジョブの削除*

    PrintPhotoUtil.JobDelete (printerName, OutputHandler, ErrorOutputHanlder, Process_Exit);