import React from "react";

export default function ProductDetailLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
      {children}
      <h2>Featured Product</h2>
    </>
  );
}
