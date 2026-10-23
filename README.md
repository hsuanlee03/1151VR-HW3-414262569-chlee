1.	螢幕截圖
2.	GitHub連結
https://github.com/hsuanlee03/1151VR-HW3-414262569-chlee
3.	YouTube連結
https://youtu.be/NA6XNnFpLHE
4.	說明製作流程和相關操作
<GitHub>
1.開啟新的repository並命名
2.在Settings中設定共享
3.將專案儲存進去
<專案開啟>
1.開啟Unity
2.建立2D專案
<背景設置>
1.在 Hierarchy建立一個2D的Square
2.設定背景顏色
<2D物件>
1.下載輪盤、箭頭圖片
2.在Assets建立資料夾【Object】，把圖片拉入
3.將輪盤拉入畫面，圖層設為1
4.將箭頭拉入畫面，圖層設為2
<腳本>
1.在Assets建立【MonoBehavior Script】
2.寫一個使用空白鍵可以啟動轉盤的腳本，並且轉動時和停止時皆需要音效
3.寫好腳本後，把腳本拉入輪盤中
4.將下載好的音效放入輪盤的Inspector中，並把角度改成60(輪盤總共有6個區塊)
5.	困境
	將音效放入後，測試時沒有聲音。
後來我在輪盤的Inspector新增【Audio Source】，並把【Play on awake】關閉
