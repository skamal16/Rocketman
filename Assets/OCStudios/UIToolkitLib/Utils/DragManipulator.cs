using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragManipulator : PointerManipulator
{
    public DragManipulator(VisualElement target)
    {
        this.target = target;
        root = target.parent;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        //target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        //target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    private Translate targetStartPosition { get; set; }

    private Vector3 pointerStartPosition { get; set; }

    private bool enabled { get; set; }

    private VisualElement root { get; }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        targetStartPosition = target.style.translate.value;
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        //enabled = true;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        //if (enabled && target.HasPointerCapture(evt.pointerId))
        if (target.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPosition;

            target.style.translate = new Translate(
                Mathf.Clamp(targetStartPosition.x.value + pointerDelta.x, 0, root.resolvedStyle.width - target.resolvedStyle.width),
                Mathf.Clamp(targetStartPosition.y.value + pointerDelta.y, 0, root.resolvedStyle.height - target.resolvedStyle.height),
                0);
        }
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {
        //if (enabled && target.HasPointerCapture(evt.pointerId))
        if (target.HasPointerCapture(evt.pointerId))
        {
            target.ReleasePointer(evt.pointerId);
        }
    }

    //private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    //{
    //    if (enabled)
    //    {
    //        VisualElement slotsContainer = root.Q<VisualElement>("slots");
    //        UQueryBuilder<VisualElement> allSlots =
    //            slotsContainer.Query<VisualElement>(className: "slot");
    //        UQueryBuilder<VisualElement> overlappingSlots =
    //            allSlots.Where(OverlapsTarget);
    //        VisualElement closestOverlappingSlot =
    //            FindClosestSlot(overlappingSlots);
    //        Vector3 closestPos = Vector3.zero;
    //        if (closestOverlappingSlot != null)
    //        {
    //            closestPos = RootSpaceOfSlot(closestOverlappingSlot);
    //            closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
    //        }
    //        target.transform.position =
    //            closestOverlappingSlot != null ?
    //            closestPos :
    //            targetStartPosition;

    //        enabled = false;
    //    }
    //}

    //private bool OverlapsTarget(VisualElement slot)
    //{
    //    return target.worldBound.Overlaps(slot.worldBound);
    //}

    //private VisualElement FindClosestSlot(UQueryBuilder<VisualElement> slots)
    //{
    //    List<VisualElement> slotsList = slots.ToList();
    //    float bestDistanceSq = float.MaxValue;
    //    VisualElement closest = null;
    //    foreach (VisualElement slot in slotsList)
    //    {
    //        Vector3 displacement =
    //            RootSpaceOfSlot(slot) - target.transform.position;
    //        float distanceSq = displacement.sqrMagnitude;
    //        if (distanceSq < bestDistanceSq)
    //        {
    //            bestDistanceSq = distanceSq;
    //            closest = slot;
    //        }
    //    }
    //    return closest;
    //}

    //private Vector3 RootSpaceOfSlot(VisualElement slot)
    //{
    //    Vector2 slotWorldSpace = slot.parent.LocalToWorld(slot.layout.position);
    //    return root.WorldToLocal(slotWorldSpace);
    //}
}