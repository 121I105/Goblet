using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject currentPiece; // ���݂��̃}�X�ڂɑ��݂����
    private List<GameObject> deactivatedPieces = new List<GameObject>(); // ���̃}�X�ڂŔ�A�N�e�B�u�����ꂽ��̃��X�g

    // ����̃}�X�ڂɓ���Ƃ��ɌĂяo��
    public void EnterPiece(GameObject piece)
    {
        Debug.Log("Entering piece: " + piece.name);
        // ���ɋ����ꍇ
        if (currentPiece != null)
        {
            Strength currentStrength = currentPiece.GetComponent<Strength>();
            Strength newStrength = piece.GetComponent<Strength>();

            // �V��������݂̋���������ꍇ
            if (newStrength.GetStrength() > currentStrength.GetStrength())
            {
                // ���݂̋���A�N�e�B�u�����ă��X�g�ɒǉ�
                currentPiece.SetActive(false);
                deactivatedPieces.Add(currentPiece);

                // �V��������A�N�e�B�u��
                currentPiece = piece;
            }
            else
            {
                // �V��������A�N�e�B�u���i�ア�ꍇ�j
                piece.SetActive(false);
                deactivatedPieces.Add(piece);
            }
        }
        else
        {
            // �}�X�ڂ���Ȃ�V�������u��
            currentPiece = piece;
        }
    }

    // ����̃}�X�ڂ��痣���Ƃ��ɌĂяo��
    public void ExitPiece(GameObject piece)
    {
        Debug.Log("Exiting piece: " + piece.name);
        if (currentPiece == piece)
        {
            // �ł�������A�N�e�B�u�ȋ���ăA�N�e�B�u��
            ReactivateStrongestPiece();

            // ���݂̋���N���A
            currentPiece = null;
        }
        else
        {
            // �p�����[�^�̋currentPiece�łȂ��ꍇ�A���X�g����폜
            deactivatedPieces.Remove(piece);
        }
    }

    private void ReactivateStrongestPiece()
    {
        if (deactivatedPieces.Count > 0)
        {
            var strongestPiece = deactivatedPieces
                .OrderByDescending(p => p.GetComponent<Strength>().GetStrength())
                .First();

            strongestPiece.SetActive(true);
            currentPiece = strongestPiece;
            deactivatedPieces.Remove(strongestPiece);
        }
    }
}
