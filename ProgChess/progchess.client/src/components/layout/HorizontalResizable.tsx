import { useEffect, useRef, useState } from "react";

interface BoxCardProps {
  children: React.ReactNode;
  setDisabledSelect: any;
}

export default function HorizontalResizable({
  children,
  setDisabledSelect,
}: BoxCardProps) {
  const isResized = useRef(false);
  const [width, setWidth] = useState(
    parseInt(localStorage.getItem("leftWidth")!) || window.innerWidth / 2
  );

  useEffect(() => {
    localStorage.setItem("leftWidth", width.toString());
  }, [width]);

  useEffect(() => {
    window.addEventListener("mousemove", (e) => {
      if (!isResized.current) {
        return;
      }
      setWidth((previousWidth) => previousWidth + e.movementX / 2);
    });

    window.addEventListener("mouseup", () => {
      isResized.current = false;
      setDisabledSelect(false);
    });
  }, []);

  return (
    <div className="flex overflow-auto">
      <div style={{ width: `${width / 16}rem` }}>{children}</div>

      <div
        onMouseDown={() => {
          isResized.current = true;
          setDisabledSelect(true);
        }}
        className="w-2 cursor-col-resize"
      ></div>
    </div>
  );
}
