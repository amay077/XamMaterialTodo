Xamarin.Forms Visual によるマテリアルな iOS/Android アプリのサンプル
----

## これは何？

Xamarin.Forms 3.6 で **Xamarin.Forms Visual** という機能が追加されました。

* [Beautiful Material Design for Android & iOS | Xamarin Blog](https://devblogs.microsoft.com/xamarin/beautiful-material-design-android-ios/)
* [Xamarin.Forms Visual - Xamarin | Microsoft Docs](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/visual/)

このリポジトリは、この Xamarin.Forms Visual を使用して Material デザインを適用した Android/iOS 向けのサンプルアプリケーションです。

## Xamarin.Forms Visual とは何か？

Xamarin.Forms Visual は、 ``ContentPage`` や各UIパーツに存在する ``Visual`` プロパティに定義された値を指定することによって、そのUIパーツの **見た目や挙動を切り替える** 事ができます。

公式で用意された、Visual プロパティへ設定可能な値には、

* ``Default`` 
* ``MatchParent``
* ``Material``

があり、[Material](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/visual/material-visual) は、Visual への設定値の一つに過ぎません。

Visual の実態は Custom Renderer であり、``Material`` と設定された場合には、[Material 用の Custom Renderer](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/visual/material-visual#customize-material-renderers) が動作し、Material デザインのような見た目と挙動を実現しています。

見た目だけを切り替えるのであれば [Themes](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/themes/) や [Styles](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/styles/) という機能が既に存在していますが、「挙動」も含めた柔軟な UI の変更が求められる場合には Visual を使用すべきです。

## 必要な環境

### 開発環境

* Windows - Visual Studio 2017 または 2019
* macOS - Visual Studio for Mac ver 8.0.5 (作者はこちらを使用しています)

### 実行環境

* Android - Android 5.0 以降の実機端末またはエミュレータ
* iOS - iOS 8.0 以降の実機端末またはシミュレータ

※UWP には対応していません。

## 実行(ビルド)方法

Visual Studio 2017/2019 または Visual Studio for Mac で ``XamMaterialTodo.sln`` ファイルを開き、実機またはエミュレータをデプロイ先に選択して実行してください。

## どんなサンプル？

シンプルな ToDo アプリです。

### Android 版

画像

### iOS 版

画像

## Xamarin.Forms における Material デザイン適用の実際

アプリに簡単にマテリアルデザインを適用できる、という触れ込みの Xamarin.Forms Visual Material ですが、実際には満足が行く程度にまでマテリアルな見た目にするには、機能がまったく足りません。

現在対応 Material にしているUIパーツは以下の11個です。(*付きはサンプルアプリで使用しています)

* Button
* Entry(*)
* Frame
* ProgressBar
* DatePicker(*)
* TimePicker
* Picker
* ActivityIndicator
* Editor(*)
* Slider(*)
* Stepper

[Material Design](https://material.io/design/) 公式で提示されている Components や、マテリアルデザインの提供者である Google が開発しているクロスプラットフォームアプリ開発ツール [Flutter の Material Widgets](https://flutter.dev/docs/development/ui/widgets/material) と比較すると、パーツの数も再現度も貧弱と言わざるを得ません。

## サンプルアプリの設計

MVVM パターンを採用しています。DDD や Clean Archtecture から「Usecase」や「Repository」という概念も採用しています。

プラットフォーム側での特殊処理は行っておらず、共通の XamMaterialTodo プロジェクトですべての実装を行っています。

XamMaterialTodo プロジェクトのディレクトリ(名前空間)構成は以下のようになっています。

```
/XamMaterialTodo
 ├/DataModels
 ├/Repositories
 ├/Usecases
 └/Presentations
   ├/Main
   └/Detail
```

### DataModels 名前空間

プロジェクトで使用するデータクラスが含まれています。今回は一つの ToDo を表す ``TodoItem`` のみが含まれ、あらゆる箇所で使用されます。

### Repositories 名前空間

データストアから、 ``TodoItem`` 読み出し、または保存する Interface 定義とその実装クラスを含みます。

今回はデータストアに LiteDB を採用しました。

* [LiteDB :: A .NET embedded NoSQL database](https://www.litedb.org/)

端末内のデータストアといえばまずは SQLite が想定されると思いますが、SQLite は、テーブルを設計・作成したり、データのI/Oのために SQL を記述する必要があるなどの面倒さがあります。
LiteDB は。MogoDB のようなドキュメント指向の NoSQL で、``TodoItem`` をそのまま扱える利点があります。またすべて C# で実装されており、導入も簡単です。

LiteDB に対してのデータIOは ``LiteDbTodoRepository`` として実装されています。

SQLite, Firebase Firestore, AppCenter Data といった他のデータストアに対応したい場合は、 ``ITodoRepository`` インターフェースを実装して新しいリポジトリクラスを作成し、``LiteDbTodoRepository`` と差し替えるだけです。

### Usecases 名前空間

この層にはToDoアプリについてのビジネスロジックを実装したクラスが含まれます。
今回は機能の少ない単純なアプリであるため、``TodoUsecase`` クラスが一つだけあり、「ToDo の追加や削除」、「ToDo の完了」、「未完了または全ての ToDo 一覧の取得」などの機能が実装されています。もちろんその実装には ``ITodoRepository`` が使用されています。 

### Presentation 名前空間

この層には、いわゆる MVVM の V(View) と VM(ViewModel) が含まれます。

``Main`` は ToDo 一覧画面、 ``Detail`` が ToDo 詳細画面を示し、それぞれのディレクトリに画面を示す ``Page`` クラスと、 ``ViewModel`` クラスが含まれます。

#### ReactiveProperty と Reactive Extensions

Page とのデータバインディングに必要な ViewModel の INotifyPropertyChanged の実装は [ReactiveProperty](https://github.com/runceel/ReactiveProperty) を採用しています。